
static EncryptTokenWithZeroBuffer(secretKey, data) {
    try {
      const passwordHash = Crypto.createHash('md5').update(secretKey, 'utf-8').digest('hex').toUpperCase();
      const iv = new Buffer.alloc(16); // fill with zeros
      const cipher = Crypto.createCipheriv('aes-256-cbc', passwordHash, iv);
      return cipher.update(data, 'utf8', 'hex') + cipher.final('hex');
    } catch (exception) {
      return exception;
    }
  }

  // Encryption with random buffer.
  static EncryptTokenWithRandomBuffer(secretKey, iv, data) {
    try {
      const passwordHash = Crypto.createHash('md5').update(secretKey, 'utf-8').digest('hex').toUpperCase();
      const cipher = Crypto.createCipheriv('aes-256-cbc', passwordHash, iv);
      return cipher.update(data, 'utf8', 'hex') + cipher.final('hex');
    } catch (exception) {
      return exception;
    }
  }

  // Decryption with zero buffer.
  static DecryptTokenWithZeroBuffer(secretKey, data) {
    try {
      const iv = new Buffer.alloc(16);
      const passwordHash = Crypto.createHash('md5').update(secretKey, 'utf-8').digest('hex').toUpperCase();
      const decipher = Crypto.createDecipheriv('aes-256-cbc', passwordHash, iv);
      return decipher.update(data, 'hex', 'utf8') + decipher.final('utf8');
    } catch (exception) {
      return exception;
    }
  }

  // Decryption with random buffer.
  static DecryptTokenWithRandomBuffer(secretKey, iv, data) {
    try {
      const passwordHash = Crypto.createHash('md5').update(secretKey, 'utf-8').digest('hex').toUpperCase();
      const decipher = Crypto.createDecipheriv('aes-256-cbc', passwordHash, iv);
      return decipher.update(data, 'hex', 'utf8') + decipher.final('utf8');
    } catch (exception) {
      return exception;
    }
  }