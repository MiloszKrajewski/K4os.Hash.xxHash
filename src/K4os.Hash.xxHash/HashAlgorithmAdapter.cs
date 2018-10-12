using System;
using System.Security.Cryptography;

namespace K4os.Hash.xxHash
{
	public class HashAlgorithmAdapter: HashAlgorithm
	{
		private readonly Action _reset;
		private readonly Action<byte[], int, int> _update;
		private readonly Func<byte[]> _digest;

		public HashAlgorithmAdapter(
			int hashSize,
			Action reset,
			Action<byte[], int, int> update,
			Func<byte[]> digest)
		{
			_reset = reset;
			_update = update;
			_digest = digest;
			HashSize = hashSize;
		}

		public override int HashSize { get; }

#if NETSTANDARD1_6
		public byte[] Hash => _digest();
#else
		public override byte[] Hash => _digest();
#endif

		protected override void HashCore(byte[] array, int ibStart, int cbSize) =>
			_update(array, ibStart, cbSize);

		protected override byte[] HashFinal() =>
			_digest();

		public override void Initialize() =>
			_reset();

		
	}
}
