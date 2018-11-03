// ReSharper disable InconsistentNaming

using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace K4os.Hash.xxHash
{
	/// <summary>
	/// xxHash 64-bit.
	/// </summary>
	public partial class XXH64
	{
		/// <summary>Hash of provided buffer.</summary>
		/// <param name="bytes">Buffer.</param>
		/// <param name="length">Length of buffer.</param>
		/// <returns>Digest.</returns>
		public static unsafe ulong DigestOf(void* bytes, int length) =>
			XXH64_hash(bytes, length, 0);
		
		/// <summary>Hash of provided buffer.</summary>
		/// <param name="bytes">Buffer.</param>
		/// <returns>Digest.</returns>
		public static unsafe ulong DigestOf(ReadOnlySpan<byte> bytes)
		{
			fixed (byte* bytesP = &MemoryMarshal.GetReference(bytes))
				return DigestOf(bytesP, bytes.Length);
		}

		/// <summary>Hash of provided buffer.</summary>
		/// <param name="bytes">Buffer.</param>
		/// <param name="offset">Starting offset.</param>
		/// <param name="length">Length of buffer.</param>
		/// <returns>Digest.</returns>
		public static unsafe ulong DigestOf(byte[] bytes, int offset, int length)
		{
			Validate(bytes, offset, length);

			fixed (byte* bytes0 = bytes)
				return DigestOf(bytes0 + offset, length);
		}

		private XXH64_state _state;

		/// <summary>Creates xxHash instance.</summary>
		public XXH64() => Reset();

		/// <summary>Resets hash calculation.</summary>
		public unsafe void Reset()
		{
			fixed (XXH64_state* stateP = &_state)
				XXH64_reset(stateP, 0);
		}

		/// <summary>Updates the has using given buffer.</summary>
		/// <param name="bytes">Buffer.</param>
		/// <param name="length">Length of buffer.</param>
		public unsafe void Update(byte* bytes, int length)
		{
			fixed (XXH64_state* stateP = &_state)
				XXH64_update(stateP, bytes, length);
		}
		
		/// <summary>Updates the has using given buffer.</summary>
		/// <param name="bytes">Buffer.</param>
		public unsafe void Update(ReadOnlySpan<byte> bytes)
		{
			fixed (byte* bytesP = &MemoryMarshal.GetReference(bytes))
				Update(bytesP, bytes.Length);
		}

		/// <summary>Updates the has using given buffer.</summary>
		/// <param name="bytes">Buffer.</param>
		/// <param name="offset">Starting offset.</param>
		/// <param name="length">Length of buffer.</param>
		public unsafe void Update(byte[] bytes, int offset, int length)
		{
			Validate(bytes, offset, length);
			
			fixed (byte* bytesP = bytes)
				Update(bytesP + offset, length);
		}

		/// <summary>Hash so far.</summary>
		/// <returns>Hash so far.</returns>
		public unsafe ulong Digest()
		{
			fixed (XXH64_state* stateP = &_state)
				return XXH64_digest(stateP);
		}

		/// <summary>Hash so far, as byte array.</summary>
		/// <returns>Hash so far.</returns>
		public byte[] DigestBytes() => BitConverter.GetBytes(Digest());

		/// <summary>Converts this class to <see cref="HashAlgorithm"/></summary>
		/// <returns><see cref="HashAlgorithm"/></returns>
		public HashAlgorithm AsHashAlgorithm() =>
			new HashAlgorithmAdapter(sizeof(uint), Reset, Update, DigestBytes);
	}
}
