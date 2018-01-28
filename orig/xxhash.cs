#line 1 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
#line 107 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
#line 1 "D:/Projects/K4os.Hash.XXHash/orig/include/stdlib.h"
#line 108 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
static void* XXH_malloc(size_t s) { return malloc(s); }
static void XXH_free (void* p) { free(p); }
#line 111 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
#line 1 "D:/Projects/K4os.Hash.XXHash/orig/include/string.h"
#line 112 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
static void* XXH_memcpy(void* dest, const void* src, size_t size) { return memcpy(dest,src,size); }
#line 115 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
#line 1 "xxhash.h"
#line 78 "xxhash.h"
#line 1 "D:/Projects/K4os.Hash.XXHash/orig/include/stddef.h"
#line 79 "xxhash.h"
typedef enum { XXH_OK=0, XXH_ERROR } XXH_errorcode;
#line 155 "xxhash.h"
 unsigned XXH_versionNumber (void);
#line 161 "xxhash.h"
typedef unsigned int XXH32_hash_t;
#line 168 "xxhash.h"
 XXH32_hash_t XXH32 (const void* input, size_t length, unsigned int seed);


typedef struct XXH32_state_s XXH32_state_t;
 XXH32_state_t* XXH32_createState(void);
 XXH_errorcode XXH32_freeState(XXH32_state_t* statePtr);
 void XXH32_copyState(XXH32_state_t* dst_state, const XXH32_state_t* src_state);

 XXH_errorcode XXH32_reset (XXH32_state_t* statePtr, unsigned int seed);
 XXH_errorcode XXH32_update (XXH32_state_t* statePtr, const void* input, size_t length);
 XXH32_hash_t XXH32_digest (const XXH32_state_t* statePtr);
#line 204 "xxhash.h"
typedef struct { unsigned char digest[4]; } XXH32_canonical_t;
 void XXH32_canonicalFromHash(XXH32_canonical_t* dst, XXH32_hash_t hash);
 XXH32_hash_t XXH32_hashFromCanonical(const XXH32_canonical_t* src);
#line 219 "xxhash.h"
typedef unsigned long long XXH64_hash_t;
#line 226 "xxhash.h"
 XXH64_hash_t XXH64 (const void* input, size_t length, unsigned long long seed);


typedef struct XXH64_state_s XXH64_state_t;
 XXH64_state_t* XXH64_createState(void);
 XXH_errorcode XXH64_freeState(XXH64_state_t* statePtr);
 void XXH64_copyState(XXH64_state_t* dst_state, const XXH64_state_t* src_state);

 XXH_errorcode XXH64_reset (XXH64_state_t* statePtr, unsigned long long seed);
 XXH_errorcode XXH64_update (XXH64_state_t* statePtr, const void* input, size_t length);
 XXH64_hash_t XXH64_digest (const XXH64_state_t* statePtr);


typedef struct { unsigned char digest[8]; } XXH64_canonical_t;
 void XXH64_canonicalFromHash(XXH64_canonical_t* dst, XXH64_hash_t hash);
 XXH64_hash_t XXH64_hashFromCanonical(const XXH64_canonical_t* src);
#line 258 "xxhash.h"
struct XXH32_state_s {
   unsigned total_len_32;
   unsigned large_len;
   unsigned v1;
   unsigned v2;
   unsigned v3;
   unsigned v4;
   unsigned mem32[4];
   unsigned memsize;
   unsigned reserved;
};


struct XXH64_state_s {
   unsigned long long total_len;
   unsigned long long v1;
   unsigned long long v2;
   unsigned long long v3;
   unsigned long long v4;
   unsigned long long mem64[4];
   unsigned memsize;
   unsigned reserved[2];
};
#line 116 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
#line 149 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
    typedef unsigned char BYTE;
    typedef unsigned short U16;
    typedef unsigned int U32;






static U32 XXH_read32(const void* memPtr) { return *(const U32*) memPtr; }
#line 201 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
static U32 XXH_swap32 (U32 x)
{
    return ((x << 24) & 0xff000000 ) |
            ((x << 8) & 0x00ff0000 ) |
            ((x >> 8) & 0x0000ff00 ) |
            ((x >> 24) & 0x000000ff );
}
#line 214 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
typedef enum { XXH_bigEndian=0, XXH_littleEndian=1 } XXH_endianess;



    static const int g_one = 1;
#line 226 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
typedef enum { XXH_aligned, XXH_unaligned } XXH_alignment;

static  U32 XXH_readLE32_align(const void* ptr, XXH_endianess endian, XXH_alignment align)
{
    if (align==XXH_unaligned)
        return endian==XXH_littleEndian ? XXH_read32(ptr) : XXH_swap32(XXH_read32(ptr));
    else
        return endian==XXH_littleEndian ? *(const U32*)ptr : XXH_swap32(*(const U32*)ptr);
}

static  U32 XXH_readLE32(const void* ptr, XXH_endianess endian)
{
    return XXH_readLE32_align(ptr, endian, XXH_unaligned);
}

static U32 XXH_readBE32(const void* ptr)
{
    return  (*(const char*)(&g_one))  ? XXH_swap32(XXH_read32(ptr)) : XXH_read32(ptr);
}
#line 251 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
 unsigned XXH_versionNumber (void) { return  ( 0 *100*100 + 6 *100 + 4 ) ; }
#line 257 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
static const U32 PRIME32_1 = 2654435761U;
static const U32 PRIME32_2 = 2246822519U;
static const U32 PRIME32_3 = 3266489917U;
static const U32 PRIME32_4 = 668265263U;
static const U32 PRIME32_5 = 374761393U;

static U32 XXH32_round(U32 seed, U32 input)
{
    seed += input * PRIME32_2;
    seed =  ((seed << 13) | (seed >> (32 - 13))) ;
    seed *= PRIME32_1;
    return seed;
}

static  U32 XXH32_endian_align(const void* input, size_t len, U32 seed, XXH_endianess endian, XXH_alignment align)
{
    const BYTE* p = (const BYTE*)input;
    const BYTE* bEnd = p + len;
    U32 h32;
#line 285 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
    if (len>=16) {
        const BYTE* const limit = bEnd - 16;
        U32 v1 = seed + PRIME32_1 + PRIME32_2;
        U32 v2 = seed + PRIME32_2;
        U32 v3 = seed + 0;
        U32 v4 = seed - PRIME32_1;

        do {
            v1 = XXH32_round(v1,  XXH_readLE32_align(p, endian, align) ); p+=4;
            v2 = XXH32_round(v2,  XXH_readLE32_align(p, endian, align) ); p+=4;
            v3 = XXH32_round(v3,  XXH_readLE32_align(p, endian, align) ); p+=4;
            v4 = XXH32_round(v4,  XXH_readLE32_align(p, endian, align) ); p+=4;
        } while (p<=limit);

        h32 =  ((v1 << 1) | (v1 >> (32 - 1)))  +  ((v2 << 7) | (v2 >> (32 - 7)))  +  ((v3 << 12) | (v3 >> (32 - 12)))  +  ((v4 << 18) | (v4 >> (32 - 18))) ;
    } else {
        h32 = seed + PRIME32_5;
    }

    h32 += (U32) len;

    while (p+4<=bEnd) {
        h32 +=  XXH_readLE32_align(p, endian, align)  * PRIME32_3;
        h32 =  ((h32 << 17) | (h32 >> (32 - 17)))  * PRIME32_4 ;
        p+=4;
    }

    while (p<bEnd) {
        h32 += (*p) * PRIME32_5;
        h32 =  ((h32 << 11) | (h32 >> (32 - 11)))  * PRIME32_1 ;
        p++;
    }

    h32 ^= h32 >> 15;
    h32 *= PRIME32_2;
    h32 ^= h32 >> 13;
    h32 *= PRIME32_3;
    h32 ^= h32 >> 16;

    return h32;
}


 unsigned int XXH32 (const void* input, size_t len, unsigned int seed)
{







    XXH_endianess endian_detected = (XXH_endianess) (*(const char*)(&g_one)) ;

    if ( 0 ) {
        if ((((size_t)input) & 3) == 0) {
            if ((endian_detected==XXH_littleEndian) ||  0 )
                return XXH32_endian_align(input, len, seed, XXH_littleEndian, XXH_aligned);
            else
                return XXH32_endian_align(input, len, seed, XXH_bigEndian, XXH_aligned);
    } }

    if ((endian_detected==XXH_littleEndian) ||  0 )
        return XXH32_endian_align(input, len, seed, XXH_littleEndian, XXH_unaligned);
    else
        return XXH32_endian_align(input, len, seed, XXH_bigEndian, XXH_unaligned);

}





 XXH32_state_t* XXH32_createState(void)
{
    return (XXH32_state_t*)XXH_malloc(sizeof(XXH32_state_t));
}
 XXH_errorcode XXH32_freeState(XXH32_state_t* statePtr)
{
    XXH_free(statePtr);
    return XXH_OK;
}

 void XXH32_copyState(XXH32_state_t* dstState, const XXH32_state_t* srcState)
{
    memcpy(dstState, srcState, sizeof(*dstState));
}

 XXH_errorcode XXH32_reset(XXH32_state_t* statePtr, unsigned int seed)
{
    XXH32_state_t state;
    memset(&state, 0, sizeof(state));
    state.v1 = seed + PRIME32_1 + PRIME32_2;
    state.v2 = seed + PRIME32_2;
    state.v3 = seed + 0;
    state.v4 = seed - PRIME32_1;

    memcpy(statePtr, &state, sizeof(state) - sizeof(state.reserved));
    return XXH_OK;
}


static
XXH_errorcode XXH32_update_endian (XXH32_state_t* state, const void* input, size_t len, XXH_endianess endian)
{
    const BYTE* p = (const BYTE*)input;
    const BYTE* const bEnd = p + len;

    if (input==NULL)



        return XXH_ERROR;


    state->total_len_32 += (unsigned)len;
    state->large_len |= (len>=16) | (state->total_len_32>=16);

    if (state->memsize + len < 16) {
        XXH_memcpy((BYTE*)(state->mem32) + state->memsize, input, len);
        state->memsize += (unsigned)len;
        return XXH_OK;
    }

    if (state->memsize) {
        XXH_memcpy((BYTE*)(state->mem32) + state->memsize, input, 16-state->memsize);
        { const U32* p32 = state->mem32;
            state->v1 = XXH32_round(state->v1, XXH_readLE32(p32, endian)); p32++;
            state->v2 = XXH32_round(state->v2, XXH_readLE32(p32, endian)); p32++;
            state->v3 = XXH32_round(state->v3, XXH_readLE32(p32, endian)); p32++;
            state->v4 = XXH32_round(state->v4, XXH_readLE32(p32, endian));
        }
        p += 16-state->memsize;
        state->memsize = 0;
    }

    if (p <= bEnd-16) {
        const BYTE* const limit = bEnd - 16;
        U32 v1 = state->v1;
        U32 v2 = state->v2;
        U32 v3 = state->v3;
        U32 v4 = state->v4;

        do {
            v1 = XXH32_round(v1, XXH_readLE32(p, endian)); p+=4;
            v2 = XXH32_round(v2, XXH_readLE32(p, endian)); p+=4;
            v3 = XXH32_round(v3, XXH_readLE32(p, endian)); p+=4;
            v4 = XXH32_round(v4, XXH_readLE32(p, endian)); p+=4;
        } while (p<=limit);

        state->v1 = v1;
        state->v2 = v2;
        state->v3 = v3;
        state->v4 = v4;
    }

    if (p < bEnd) {
        XXH_memcpy(state->mem32, p, (size_t)(bEnd-p));
        state->memsize = (unsigned)(bEnd-p);
    }

    return XXH_OK;
}

 XXH_errorcode XXH32_update (XXH32_state_t* state_in, const void* input, size_t len)
{
    XXH_endianess endian_detected = (XXH_endianess) (*(const char*)(&g_one)) ;

    if ((endian_detected==XXH_littleEndian) ||  0 )
        return XXH32_update_endian(state_in, input, len, XXH_littleEndian);
    else
        return XXH32_update_endian(state_in, input, len, XXH_bigEndian);
}



static  U32 XXH32_digest_endian (const XXH32_state_t* state, XXH_endianess endian)
{
    const BYTE * p = (const BYTE*)state->mem32;
    const BYTE* const bEnd = (const BYTE*)(state->mem32) + state->memsize;
    U32 h32;

    if (state->large_len) {
        h32 =  ((state->v1 << 1) | (state->v1 >> (32 - 1)))
            +  ((state->v2 << 7) | (state->v2 >> (32 - 7)))
            +  ((state->v3 << 12) | (state->v3 >> (32 - 12)))
            +  ((state->v4 << 18) | (state->v4 >> (32 - 18))) ;
    } else {
        h32 = state->v3 + PRIME32_5;
    }

    h32 += state->total_len_32;

    while (p+4<=bEnd) {
        h32 += XXH_readLE32(p, endian) * PRIME32_3;
        h32 =  ((h32 << 17) | (h32 >> (32 - 17)))  * PRIME32_4;
        p+=4;
    }

    while (p<bEnd) {
        h32 += (*p) * PRIME32_5;
        h32 =  ((h32 << 11) | (h32 >> (32 - 11)))  * PRIME32_1;
        p++;
    }

    h32 ^= h32 >> 15;
    h32 *= PRIME32_2;
    h32 ^= h32 >> 13;
    h32 *= PRIME32_3;
    h32 ^= h32 >> 16;

    return h32;
}


 unsigned int XXH32_digest (const XXH32_state_t* state_in)
{
    XXH_endianess endian_detected = (XXH_endianess) (*(const char*)(&g_one)) ;

    if ((endian_detected==XXH_littleEndian) ||  0 )
        return XXH32_digest_endian(state_in, XXH_littleEndian);
    else
        return XXH32_digest_endian(state_in, XXH_bigEndian);
}
#line 519 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
 void XXH32_canonicalFromHash(XXH32_canonical_t* dst, XXH32_hash_t hash)
{
    { enum { XXH_sa = 1/(int)(!!(sizeof(XXH32_canonical_t) == sizeof(XXH32_hash_t))) }; } ;
    if ( (*(const char*)(&g_one)) ) hash = XXH_swap32(hash);
    memcpy(dst, &hash, sizeof(*dst));
}

 XXH32_hash_t XXH32_hashFromCanonical(const XXH32_canonical_t* src)
{
    return XXH_readBE32(src);
}
#line 549 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
    typedef unsigned long long U64;







static U64 XXH_read64(const void* memPtr) { return *(const U64*) memPtr; }
#line 586 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
static U64 XXH_swap64 (U64 x)
{
    return ((x << 56) & 0xff00000000000000ULL) |
            ((x << 40) & 0x00ff000000000000ULL) |
            ((x << 24) & 0x0000ff0000000000ULL) |
            ((x << 8) & 0x000000ff00000000ULL) |
            ((x >> 8) & 0x00000000ff000000ULL) |
            ((x >> 24) & 0x0000000000ff0000ULL) |
            ((x >> 40) & 0x000000000000ff00ULL) |
            ((x >> 56) & 0x00000000000000ffULL);
}


static  U64 XXH_readLE64_align(const void* ptr, XXH_endianess endian, XXH_alignment align)
{
    if (align==XXH_unaligned)
        return endian==XXH_littleEndian ? XXH_read64(ptr) : XXH_swap64(XXH_read64(ptr));
    else
        return endian==XXH_littleEndian ? *(const U64*)ptr : XXH_swap64(*(const U64*)ptr);
}

static  U64 XXH_readLE64(const void* ptr, XXH_endianess endian)
{
    return XXH_readLE64_align(ptr, endian, XXH_unaligned);
}

static U64 XXH_readBE64(const void* ptr)
{
    return  (*(const char*)(&g_one))  ? XXH_swap64(XXH_read64(ptr)) : XXH_read64(ptr);
}




static const U64 PRIME64_1 = 11400714785074694791ULL;
static const U64 PRIME64_2 = 14029467366897019727ULL;
static const U64 PRIME64_3 = 1609587929392839161ULL;
static const U64 PRIME64_4 = 9650029242287828579ULL;
static const U64 PRIME64_5 = 2870177450012600261ULL;

static U64 XXH64_round(U64 acc, U64 input)
{
    acc += input * PRIME64_2;
    acc =  ((acc << 31) | (acc >> (64 - 31))) ;
    acc *= PRIME64_1;
    return acc;
}

static U64 XXH64_mergeRound(U64 acc, U64 val)
{
    val = XXH64_round(0, val);
    acc ^= val;
    acc = acc * PRIME64_1 + PRIME64_4;
    return acc;
}

static  U64 XXH64_endian_align(const void* input, size_t len, U64 seed, XXH_endianess endian, XXH_alignment align)
{
    const BYTE* p = (const BYTE*)input;
    const BYTE* bEnd = p + len;
    U64 h64;
#line 656 "D:/Projects/K4os.Hash.XXHash/orig/xxhash.c"
    if (len>=32) {
        const BYTE* const limit = bEnd - 32;
        U64 v1 = seed + PRIME64_1 + PRIME64_2;
        U64 v2 = seed + PRIME64_2;
        U64 v3 = seed + 0;
        U64 v4 = seed - PRIME64_1;

        do {
            v1 = XXH64_round(v1,  XXH_readLE64_align(p, endian, align) ); p+=8;
            v2 = XXH64_round(v2,  XXH_readLE64_align(p, endian, align) ); p+=8;
            v3 = XXH64_round(v3,  XXH_readLE64_align(p, endian, align) ); p+=8;
            v4 = XXH64_round(v4,  XXH_readLE64_align(p, endian, align) ); p+=8;
        } while (p<=limit);

        h64 =  ((v1 << 1) | (v1 >> (64 - 1)))  +  ((v2 << 7) | (v2 >> (64 - 7)))  +  ((v3 << 12) | (v3 >> (64 - 12)))  +  ((v4 << 18) | (v4 >> (64 - 18))) ;
        h64 = XXH64_mergeRound(h64, v1);
        h64 = XXH64_mergeRound(h64, v2);
        h64 = XXH64_mergeRound(h64, v3);
        h64 = XXH64_mergeRound(h64, v4);

    } else {
        h64 = seed + PRIME64_5;
    }

    h64 += (U64) len;

    while (p+8<=bEnd) {
        U64 const k1 = XXH64_round(0,  XXH_readLE64_align(p, endian, align) );
        h64 ^= k1;
        h64 =  ((h64 << 27) | (h64 >> (64 - 27)))  * PRIME64_1 + PRIME64_4;
        p+=8;
    }

    if (p+4<=bEnd) {
        h64 ^= (U64)( XXH_readLE32_align(p, endian, align) ) * PRIME64_1;
        h64 =  ((h64 << 23) | (h64 >> (64 - 23)))  * PRIME64_2 + PRIME64_3;
        p+=4;
    }

    while (p<bEnd) {
        h64 ^= (*p) * PRIME64_5;
        h64 =  ((h64 << 11) | (h64 >> (64 - 11)))  * PRIME64_1;
        p++;
    }

    h64 ^= h64 >> 33;
    h64 *= PRIME64_2;
    h64 ^= h64 >> 29;
    h64 *= PRIME64_3;
    h64 ^= h64 >> 32;

    return h64;
}


 unsigned long long XXH64 (const void* input, size_t len, unsigned long long seed)
{







    XXH_endianess endian_detected = (XXH_endianess) (*(const char*)(&g_one)) ;

    if ( 0 ) {
        if ((((size_t)input) & 7)==0) {
            if ((endian_detected==XXH_littleEndian) ||  0 )
                return XXH64_endian_align(input, len, seed, XXH_littleEndian, XXH_aligned);
            else
                return XXH64_endian_align(input, len, seed, XXH_bigEndian, XXH_aligned);
    } }

    if ((endian_detected==XXH_littleEndian) ||  0 )
        return XXH64_endian_align(input, len, seed, XXH_littleEndian, XXH_unaligned);
    else
        return XXH64_endian_align(input, len, seed, XXH_bigEndian, XXH_unaligned);

}



 XXH64_state_t* XXH64_createState(void)
{
    return (XXH64_state_t*)XXH_malloc(sizeof(XXH64_state_t));
}
 XXH_errorcode XXH64_freeState(XXH64_state_t* statePtr)
{
    XXH_free(statePtr);
    return XXH_OK;
}

 void XXH64_copyState(XXH64_state_t* dstState, const XXH64_state_t* srcState)
{
    memcpy(dstState, srcState, sizeof(*dstState));
}

 XXH_errorcode XXH64_reset(XXH64_state_t* statePtr, unsigned long long seed)
{
    XXH64_state_t state;
    memset(&state, 0, sizeof(state));
    state.v1 = seed + PRIME64_1 + PRIME64_2;
    state.v2 = seed + PRIME64_2;
    state.v3 = seed + 0;
    state.v4 = seed - PRIME64_1;

    memcpy(statePtr, &state, sizeof(state) - sizeof(state.reserved));
    return XXH_OK;
}

static
XXH_errorcode XXH64_update_endian (XXH64_state_t* state, const void* input, size_t len, XXH_endianess endian)
{
    const BYTE* p = (const BYTE*)input;
    const BYTE* const bEnd = p + len;

    if (input==NULL)



        return XXH_ERROR;


    state->total_len += len;

    if (state->memsize + len < 32) {
        XXH_memcpy(((BYTE*)state->mem64) + state->memsize, input, len);
        state->memsize += (U32)len;
        return XXH_OK;
    }

    if (state->memsize) {
        XXH_memcpy(((BYTE*)state->mem64) + state->memsize, input, 32-state->memsize);
        state->v1 = XXH64_round(state->v1, XXH_readLE64(state->mem64+0, endian));
        state->v2 = XXH64_round(state->v2, XXH_readLE64(state->mem64+1, endian));
        state->v3 = XXH64_round(state->v3, XXH_readLE64(state->mem64+2, endian));
        state->v4 = XXH64_round(state->v4, XXH_readLE64(state->mem64+3, endian));
        p += 32-state->memsize;
        state->memsize = 0;
    }

    if (p+32 <= bEnd) {
        const BYTE* const limit = bEnd - 32;
        U64 v1 = state->v1;
        U64 v2 = state->v2;
        U64 v3 = state->v3;
        U64 v4 = state->v4;

        do {
            v1 = XXH64_round(v1, XXH_readLE64(p, endian)); p+=8;
            v2 = XXH64_round(v2, XXH_readLE64(p, endian)); p+=8;
            v3 = XXH64_round(v3, XXH_readLE64(p, endian)); p+=8;
            v4 = XXH64_round(v4, XXH_readLE64(p, endian)); p+=8;
        } while (p<=limit);

        state->v1 = v1;
        state->v2 = v2;
        state->v3 = v3;
        state->v4 = v4;
    }

    if (p < bEnd) {
        XXH_memcpy(state->mem64, p, (size_t)(bEnd-p));
        state->memsize = (unsigned)(bEnd-p);
    }

    return XXH_OK;
}

 XXH_errorcode XXH64_update (XXH64_state_t* state_in, const void* input, size_t len)
{
    XXH_endianess endian_detected = (XXH_endianess) (*(const char*)(&g_one)) ;

    if ((endian_detected==XXH_littleEndian) ||  0 )
        return XXH64_update_endian(state_in, input, len, XXH_littleEndian);
    else
        return XXH64_update_endian(state_in, input, len, XXH_bigEndian);
}

static  U64 XXH64_digest_endian (const XXH64_state_t* state, XXH_endianess endian)
{
    const BYTE * p = (const BYTE*)state->mem64;
    const BYTE* const bEnd = (const BYTE*)state->mem64 + state->memsize;
    U64 h64;

    if (state->total_len >= 32) {
        U64 const v1 = state->v1;
        U64 const v2 = state->v2;
        U64 const v3 = state->v3;
        U64 const v4 = state->v4;

        h64 =  ((v1 << 1) | (v1 >> (64 - 1)))  +  ((v2 << 7) | (v2 >> (64 - 7)))  +  ((v3 << 12) | (v3 >> (64 - 12)))  +  ((v4 << 18) | (v4 >> (64 - 18))) ;
        h64 = XXH64_mergeRound(h64, v1);
        h64 = XXH64_mergeRound(h64, v2);
        h64 = XXH64_mergeRound(h64, v3);
        h64 = XXH64_mergeRound(h64, v4);
    } else {
        h64 = state->v3 + PRIME64_5;
    }

    h64 += (U64) state->total_len;

    while (p+8<=bEnd) {
        U64 const k1 = XXH64_round(0, XXH_readLE64(p, endian));
        h64 ^= k1;
        h64 =  ((h64 << 27) | (h64 >> (64 - 27)))  * PRIME64_1 + PRIME64_4;
        p+=8;
    }

    if (p+4<=bEnd) {
        h64 ^= (U64)(XXH_readLE32(p, endian)) * PRIME64_1;
        h64 =  ((h64 << 23) | (h64 >> (64 - 23)))  * PRIME64_2 + PRIME64_3;
        p+=4;
    }

    while (p<bEnd) {
        h64 ^= (*p) * PRIME64_5;
        h64 =  ((h64 << 11) | (h64 >> (64 - 11)))  * PRIME64_1;
        p++;
    }

    h64 ^= h64 >> 33;
    h64 *= PRIME64_2;
    h64 ^= h64 >> 29;
    h64 *= PRIME64_3;
    h64 ^= h64 >> 32;

    return h64;
}

 unsigned long long XXH64_digest (const XXH64_state_t* state_in)
{
    XXH_endianess endian_detected = (XXH_endianess) (*(const char*)(&g_one)) ;

    if ((endian_detected==XXH_littleEndian) ||  0 )
        return XXH64_digest_endian(state_in, XXH_littleEndian);
    else
        return XXH64_digest_endian(state_in, XXH_bigEndian);
}




 void XXH64_canonicalFromHash(XXH64_canonical_t* dst, XXH64_hash_t hash)
{
    { enum { XXH_sa = 1/(int)(!!(sizeof(XXH64_canonical_t) == sizeof(XXH64_hash_t))) }; } ;
    if ( (*(const char*)(&g_one)) ) hash = XXH_swap64(hash);
    memcpy(dst, &hash, sizeof(*dst));
}

 XXH64_hash_t XXH64_hashFromCanonical(const XXH64_canonical_t* src)
{
    return XXH_readBE64(src);
}
