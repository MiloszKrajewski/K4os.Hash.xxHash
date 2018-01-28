using System;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using _XXH = System.Data.HashFunction.xxHash;

// ReSharper disable InconsistentNaming

namespace K4os.Hash.xxHash.Test
{
	public class XXH64Tests
	{
		private readonly ITestOutputHelper _output;

		public XXH64Tests(ITestOutputHelper output)
		{
			_output = output;
		}

		public ulong Theirs64(byte[] bytes) =>
			BitConverter.ToUInt64(new _XXH(sizeof(ulong) * 8).ComputeHash(bytes), 0);

		[Theory]
		[InlineData("hello world")]
		[InlineData("")]
		[InlineData("quick brown fox jumped over the lazy dog")]
		[InlineData("thirteenchars")]
		public void SingleBlockXxh32MatchesTheirs(string text)
		{
			var input = Encoding.UTF8.GetBytes(text);
			var expected = Theirs64(input);
			var actual = XXH64.DigestOf(input, 0, input.Length);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(0, 17241709254077376921ul)]
		[InlineData(1, 16804241149081757544ul)]
		[InlineData(2, 5216751715308240086ul)]
		[InlineData(3, 16557408460946040285ul)]
		[InlineData(4, 18432908232848821278ul)]
		[InlineData(5, 15925419018050470668ul)]
		[InlineData(6, 4867868962443297827ul)]
		[InlineData(7, 1498682999415010002ul)]
		[InlineData(8, 9820687458478070669ul)]
		public void HashMatchesPregeneratedHashesForSmallBlocks(int length, ulong expected)
		{
			var bytes = new byte[length];
			for (var i = 0; i < bytes.Length; i++) bytes[i] = (byte) i;

			var actual = XXH64.DigestOf(bytes, 0, bytes.Length);
			var confirm = Theirs64(bytes);

			Assert.Equal(expected, confirm);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(1000, 33)]
		[InlineData(1000, 13)]
		[InlineData(1000, 7)]
		[InlineData(1000, 27)]
		public void CalculatingHashInChunksReturnsSameResultAsInOneGo(int length, int chuck)
		{
			var bytes = new byte[length];
			var random = new Random(0);
			random.NextBytes(bytes);

			var transform = new XXH64();
			var i = 0;
			while (i < length)
			{
				var l = Math.Min(chuck, length - i);
				transform.Update(bytes, i, l);
				i += l;
			}

			Assert.Equal(XXH64.DigestOf(bytes, 0, bytes.Length), transform.Digest());
		}
	}
}