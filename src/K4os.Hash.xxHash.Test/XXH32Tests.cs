using System;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using _XXH = System.Data.HashFunction.xxHash;

// ReSharper disable InconsistentNaming

namespace K4os.Hash.xxHash.Test
{
	public class XXH32Tests
	{
		private readonly ITestOutputHelper _output;

		public XXH32Tests(ITestOutputHelper output)
		{
			_output = output;
		}

		[Theory]
		[InlineData("hello world")]
		[InlineData("")]
		[InlineData("quick brown fox jumped over the lazy dog")]
		[InlineData("thirteenchars")]
		public void SingleBlockXxh32MatchesTheirs(string text)
		{
			var input = Encoding.UTF8.GetBytes(text);
			var expected = Theirs32(input);
			var actual = XXH32.DigestOf(input, 0, input.Length);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(0, 10, 3)]
		[InlineData(1, 100, 33)]
		[InlineData(2, 100, 5)]
		[InlineData(3, 100, 13)]
		[InlineData(4, 100, 1000)]
		public void RestartableHashReturnsSameResultsAsSingleBlock(int seed, int length, int chunk)
		{
			var random = new Random(seed);
			var bytes = new byte[length];
			random.NextBytes(bytes);

			var expected = XXH32.DigestOf(bytes, 0, bytes.Length);

			var transform = new XXH32();
			var index = 0;
			while (index < length)
			{
				var l = Math.Min(chunk, length - index);
				transform.Update(bytes, index, l);
				index += l;
			}

			var actual = transform.Digest();

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(0, 100)]
		[InlineData(1, 200)]
		[InlineData(2, 300)]
		public void EveryCallToDigestReturnsSameHash(int seed, int length)
		{
			var random = new Random(seed);
			var bytes = new byte[length];
			random.NextBytes(bytes);

			var expected = XXH32.DigestOf(bytes, 0, bytes.Length);

			var transform = new XXH32();
			transform.Update(bytes, 0, length);

			for (var i = 0; i < 100; i++)
				Assert.Equal(expected, transform.Digest());
		}

		[Theory]
		[InlineData(0, 100)]
		[InlineData(1, 200)]
		[InlineData(2, 300)]
		public void HashAlgorithmWrapperReturnsSameResults(int seed, int length)
		{
			var bytes = new byte[length];
			new Random(seed).NextBytes(bytes);

			var expected = XXH32.DigestOf(bytes, 0, bytes.Length);
			var actual = new XXH32().AsHashAlgorithm().ComputeHash(bytes);

			Assert.Equal(expected, BitConverter.ToUInt32(actual, 0));
		}

		public uint Theirs32(byte[] bytes) =>
			BitConverter.ToUInt32(new _XXH(sizeof(uint) * 8).ComputeHash(bytes), 0);
	}
}
