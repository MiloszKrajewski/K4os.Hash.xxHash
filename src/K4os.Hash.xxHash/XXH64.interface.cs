// ReSharper disable InconsistentNaming

using System;
using System.Security.Cryptography;

namespace K4os.Hash.xxHash
{
	public partial class XXH64
	{
		public static unsafe ulong DigestOf(void* bytes, int length) =>
			XXH64_hash(bytes, length, 0);

		public static unsafe ulong DigestOf(byte[] bytes, int index, int length)
		{
			fixed (byte* bytes0 = bytes)
				return XXH64_hash(bytes0 + index, length, 0);
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

		public unsafe void Update(byte[] array, int index, int length)
		{
			if (length <= 0)
				return;

			fixed (XXH64_state* stateP = &_state)
			fixed (byte* bytesP = &array[index])
				XXH64_update(stateP, bytesP, length);
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
