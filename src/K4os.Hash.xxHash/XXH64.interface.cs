// ReSharper disable InconsistentNaming

using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace K4os.Hash.xxHash
{
	public partial class XXH64
	{
		public static unsafe ulong DigestOf(void* bytes, int length) =>
			XXH64_hash(bytes, length, 0);
		
		public static unsafe ulong DigestOf(ReadOnlySpan<byte> bytes)
		{
			fixed (byte* bytesP = &MemoryMarshal.GetReference(bytes))
				return DigestOf(bytesP, bytes.Length);
		}

		public static unsafe ulong DigestOf(byte[] bytes, int index, int length)
		{
			fixed (byte* bytes0 = bytes)
				return DigestOf(bytes0 + index, length);
		}

		private XXH64_state _state;

		public XXH64()
		{
			Reset();
		}

		public unsafe void Reset()
		{
			fixed (XXH64_state* stateP = &_state)
				XXH64_reset(stateP, 0);
		}

		public unsafe void Update(byte* bytes, int length)
		{
			fixed (XXH64_state* stateP = &_state)
				XXH64_update(stateP, bytes, length);
		}
		
		public unsafe void Update(ReadOnlySpan<byte> bytes)
		{
			fixed (byte* bytesP = &MemoryMarshal.GetReference(bytes))
				Update(bytesP, bytes.Length);
		}

		public unsafe void Update(byte[] array, int index, int length)
		{
			if (length <= 0)
				return;

			fixed (byte* bytesP = &array[index])
				Update(bytesP, length);
		}

		public unsafe ulong Digest()
		{
			fixed (XXH64_state* stateP = &_state)
				return XXH64_digest(stateP);
		}

		public byte[] DigestBytes() => BitConverter.GetBytes(Digest());

		public HashAlgorithm AsHashAlgorithm() =>
			new HashAlgorithmAdapter(sizeof(uint), Reset, Update, DigestBytes);
	}
}
