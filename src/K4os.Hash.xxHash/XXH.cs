// ReSharper disable InconsistentNaming

using System.Runtime.CompilerServices;

namespace K4os.Hash.xxHash
{
	public unsafe class XXH
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static uint XXH_read32(void* p) => *(uint*) p;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static ulong XXH_read64(void* p) => *(ulong*) p;

		internal static void XXH_zero(void* target, int length)
		{
			var targetP = (byte*) target;

			while (length >= sizeof(ulong))
			{
				*(ulong*) targetP = 0;
				targetP += sizeof(ulong);
				length -= sizeof(ulong);
			}

			if (length >= sizeof(uint))
			{
				*(uint*) targetP = 0;
				targetP += sizeof(uint);
				length -= sizeof(uint);
			}
			
			if (length >= sizeof(ushort))
			{
				*(ushort*) targetP = 0;
				targetP += sizeof(ushort);
				length -= sizeof(ushort);
			}

			if (length > 0)
			{
				*targetP = 0;
				// targetP++;
				// length--;
			}
		}

		internal static void XXH_copy(void* target, void* source, int length)
		{
			var sourceP = (byte*) source;
			var targetP = (byte*) target;

			while (length >= sizeof(ulong))
			{
				*(ulong*) targetP = *(ulong*) sourceP;
				targetP += sizeof(ulong);
				sourceP += sizeof(ulong);
				length -= sizeof(ulong);
			}

			if (length >= sizeof(uint))
			{
				*(uint*) targetP = *(uint*) sourceP;
				targetP += sizeof(uint);
				sourceP += sizeof(uint);
				length -= sizeof(uint);
			}
			
			if (length >= sizeof(ushort))
			{
				*(ushort*) targetP = *(ushort*) sourceP;
				targetP += sizeof(ushort);
				sourceP += sizeof(ushort);
				length -= sizeof(ushort);
			}


			if (length > 0)
			{
				*targetP = *sourceP;
				// targetP++;
				// sourceP++;
				// length--;
			}
		}
	}
}
