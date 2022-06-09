// ReSharper disable InconsistentNaming

using System;
using System.Security.Cryptography;

namespace K4os.Hash.xxHash
{
	/// <summary>
	/// xxHash 32-bit.
	/// </summary>
	public partial class XXH32
	{
		/// <summary>Hash of empty buffer.</summary>
		public const uint EmptyHash = 46947589;

		/// <summary>Hash of provided buffer.</summary>
		/// <param name="bytes">Buffer.</param>
		/// <param name="length">Length of buffer.</param>
		/// <returns>Digest.</returns>
		public static unsafe uint DigestOf(void* bytes, int length, uint seed = 0) =>
			XXH32_hash(bytes, length, seed);

		/// <summary>Hash of provided buffer.</summary>
		/// <param name="bytes">Buffer.</param>
		/// <returns>Digest.</returns>
		public static unsafe uint DigestOf(ReadOnlySpan<byte> bytes, uint seed = 0)
		{
			fixed (byte* bytesP = bytes)
				return DigestOf(bytesP, bytes.Length, seed);
		}

		/// <summary>Hash of provided buffer.</summary>
		/// <param name="bytes">Buffer.</param>
		/// <param name="offset">Starting offset.</param>
		/// <param name="length">Length of buffer.</param>
		/// <returns>Digest.</returns>
		public static unsafe uint DigestOf(byte[] bytes, int offset, int length, uint seed = 0)
		{
			Validate(bytes, offset, length);

			fixed (byte* bytes0 = bytes)
				return DigestOf(bytes0 + offset, length, seed);
		}

		private XXH32_state _state;

		/// <summary>Creates xxHash instance.</summary>
		public XXH32() => Reset();

		/// <summary>Resets hash calculation.</summary>
		public unsafe void Reset()
		{
			fixed (XXH32_state* stateP = &_state)
				XXH32_reset(stateP, 0);
		}
		
		/// <summary>Updates the has using given buffer.</summary>
		/// <param name="bytes">Buffer.</param>
		/// <param name="length">Length of buffer.</param>
		public unsafe void Update(byte* bytes, int length)
		{
			fixed (XXH32_state* stateP = &_state)
				XXH32_update(stateP, bytes, length);
		}
		
		/// <summary>Updates the has using given buffer.</summary>
		/// <param name="bytes">Buffer.</param>
		public unsafe void Update(ReadOnlySpan<byte> bytes)
		{
			fixed (byte* bytesP = bytes)
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
		public unsafe uint Digest()
		{
			fixed (XXH32_state* stateP = &_state)
				return XXH32_digest(stateP);
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
