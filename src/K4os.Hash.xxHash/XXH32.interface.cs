// ReSharper disable InconsistentNaming

using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace K4os.Hash.xxHash
{
	public partial class XXH32
	{
		public const uint EmptyHash = 46947589;

		public static unsafe uint DigestOf(void* bytes, int length) =>
			XXH32_hash(bytes, length, 0);

		public static unsafe uint DigestOf(ReadOnlySpan<byte> bytes)
		{
			fixed (byte* bytesP = &MemoryMarshal.GetReference(bytes))
				return DigestOf(bytesP, bytes.Length);
		}

		public static unsafe uint DigestOf(byte[] bytes, int offset, int length)
		{
			Validate(bytes, offset, length);

			fixed (byte* bytes0 = bytes)
				return DigestOf(bytes0 + offset, length);
		}

		private XXH32_state _state;

		public XXH32()
		{
			Reset();
		}

		public unsafe void Reset()
		{
			fixed (XXH32_state* stateP = &_state)
				XXH32_reset(stateP, 0);
		}
		
		public unsafe void Update(byte* bytes, int length)
		{
			fixed (XXH32_state* stateP = &_state)
				XXH32_update(stateP, bytes, length);
		}
		
		public unsafe void Update(ReadOnlySpan<byte> bytes)
		{
			fixed (byte* bytesP = &MemoryMarshal.GetReference(bytes))
				Update(bytesP, bytes.Length);
		}

		public unsafe void Update(byte[] bytes, int offset, int length)
		{
			Validate(bytes, offset, length);
			
			fixed (byte* bytesP = bytes)
				Update(bytesP + offset, length);
		}

		public unsafe uint Digest()
		{
			fixed (XXH32_state* stateP = &_state)
				return XXH32_digest(stateP);
		}

		public byte[] DigestBytes() => BitConverter.GetBytes(Digest());

		public HashAlgorithm AsHashAlgorithm() =>
			new HashAlgorithmAdapter(sizeof(uint), Reset, Update, DigestBytes);
	}
}
