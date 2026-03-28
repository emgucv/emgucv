// Implementations for __wasm_setjmp and __wasm_setjmp_test that are required
// when code compiled without -fwasm-exceptions is linked with -fwasm-exceptions.
// The LLVM WebAssemblyLowerEmscriptenEHSjLj pass generates calls to these
// symbols during LTO but Emscripten 3.1.56 does not supply them.
//
// __wasm_setjmp(buf, id, tag_ptr):
//   Stores the jmp_buf address (buf) into the local tag slot (*tag_ptr),
//   so that __wasm_setjmp_test can later match a longjmp target.
//
// __wasm_setjmp_test(env, tag_ptr) -> i32:
//   Returns 1 if env (the jmp_buf pointer supplied to longjmp) equals the
//   value previously stored by __wasm_setjmp, 0 otherwise.
mergeInto(LibraryManager.library, {
    __wasm_setjmp: function(buf, id, tag_ptr) {
        HEAP32[tag_ptr >> 2] = buf;
    },
    __wasm_setjmp_test: function(env, tag_ptr) {
        return HEAP32[tag_ptr >> 2] === env ? 1 : 0;
    }
});
