﻿using System;
using System.Security.Cryptography;

namespace K4os.Hash.xxHash
{
	/// <summary>
	/// Adapter implementing <see cref="HashAlgorithm"/>
	/// </summary>
	public class HashAlgorithmAdapter: HashAlgorithm
	{
		private readonly Action _reset;
		private readonly Action<byte[], int, int> _update;
		private readonly Func<byte[]> _digest;

		/// <summary>
		/// Creates new <see cref="HashAlgorithmAdapter"/>. 
		/// </summary>
		/// <param name="hashSize">Hash size (in bytes)</param>
		/// <param name="reset">Reset function.</param>
		/// <param name="update">Update function.</param>
		/// <param name="digest">Digest function.</param>
		public HashAlgorithmAdapter(
			int hashSize, Action reset, Action<byte[], int, int> update, Func<byte[]> digest)
		{
			_reset = reset;
			_update = update;
			_digest = digest;
			HashSize = hashSize;
		}

		/// <inheritdoc />
		public override int HashSize { get; }

		#if NETSTANDARD1_6
		/// <summary>Current hash.</summary>
		public byte[] Hash => _digest();
		#else
		/// <inheritdoc />
		public override byte[] Hash => _digest();
		#endif

		/// <inheritdoc />
		protected override void HashCore(byte[] array, int ibStart, int cbSize) =>
			_update(array, ibStart, cbSize);

		/// <inheritdoc />
		protected override byte[] HashFinal() =>
			_digest();

		/// <inheritdoc />
		public override void Initialize() =>
			_reset();
	}
}
