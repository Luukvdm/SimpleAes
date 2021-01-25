# SimpleAes

- [X] Key expansion
- [X] Encrypting
- [ ] Decrypting
- [ ] Input parsing
- [ ] Unit tests

Simple AES implementation written in C#.
Written for fun, not to be used in production environments.

**Usage:**  
```csharp
byte[] message = {0x54, 0x68, 0x61, 0x74, 0x73, 0x20, 0x6d, 0x79, 0x20, 0x4b, 0x75, 0x6e, 0x67, 0x20, 0x46, 0x75};
byte[] key = { 0x54, 0x77, 0x6f, 0x20, 0x4f, 0x6e, 0x65, 0x20, 0x4e, 0x69, 0x6e, 0x65, 0x20, 0x54, 0x77, 0x6f };

Aes aes = new Aes(key);
byte[] cipherText = aes.Encrypt(message);

```
