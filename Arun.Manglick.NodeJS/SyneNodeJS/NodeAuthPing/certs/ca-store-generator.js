#!/usr/bin/env node

var fs = require('fs');
var path = require('path');
var request = require('request');

var CERTDB_URL =
  'https://mxr.mozilla.org/nss/source/lib/ckfw/builtins/certdata.txt?raw=1';

var OUTFILE = './certificates.js';

var HEADER =
  "/**\n" +
  " * Mozilla's root CA store\n" +
  " *\n" +
  " * generated from " + CERTDB_URL + "\n" +
  " */\n\n";

function Certificate() {
  this.name = null;
  this.body = '';
  this.trusted = true;
}

Certificate.prototype.quasiPEM = function quasiPEM() {
  var bytes = this.body.split('\\');
  var offset = 0;

  bytes.shift();
  var converted = new Buffer(bytes.length);
  while(bytes.length > 0) {
    converted.writeUInt8(parseInt(bytes.shift(), 8), offset++);
  }

  return '  // ' + this.name + '\n' +
         '  "-----BEGIN CERTIFICATE-----\\n" +\n' +
         converted.toString('base64').replace(/(.{1,76})/g, '  "$1\\n" +\n') +
         '  "-----END CERTIFICATE-----\\n"';
};

function parseBody(current, lines) {
  var line;

  while (lines.length > 0) {
    line = lines.shift();
    if (line.match(/^END/)) break;
    current.body += line;
  }

  while (lines.length > 0) {
    line = lines.shift();
    if (line.match(/^CKA_CLASS CK_OBJECT_CLASS CKO_NSS_TRUST/)) break;
  }

  while (lines.length > 0) {
    line = lines.shift();
    if (line.match(/^#|^\s*$/)) break;
    if (line.match(/^CKA_TRUST_SERVER_AUTH\s+CK_TRUST\s+CKT_NSS_NOT_TRUSTED$/) ||
        line.match(/^CKA_TRUST_SERVER_AUTH\s+CK_TRUST\s+CKT_NSS_TRUST_UNKNOWN$/)) {
      current.trusted = false;
    }
  }

  if (current.trusted) return current;
}

function parseCertData(lines) {
  var certs = [];
  var line;
  var current;
  var skipped = 0;

  while (lines.length > 0) {
    line = lines.shift();

    // nuke whitespace and comments
    if (line.match(/^#|^\s*$/)) continue;

    if (line.match(/^CKA_CLASS CK_OBJECT_CLASS CKO_CERTIFICATE/)) {
      current = new Certificate();
    }

    if (current) {
      var match;
      match = line.match(/^CKA_LABEL UTF8 \"(.*)\"/);
      if (match) {
        current.name = match[1];
      }

      if (line.match(/^CKA_VALUE MULTILINE_OCTAL/)) {
        var finished = parseBody(current, lines);
        if (finished) {
          certs.push(finished);
        }
        else {
          skipped++;
        }
        current = null;
      }
    }
  }

  console.error("Skipped %s untrusted certificates.", skipped);
  console.error("Processed %s certificates.", certs.length);

  return certs;
}

function dumpCerts(certs) {
  fs.writeFileSync(
    OUTFILE,
    HEADER +
    'module.exports = [\n' +
    certs.map(function (cert) { return cert.quasiPEM(); }).join(',\n\n') +
    '];\n'
  );
}

request(CERTDB_URL, function (error, response, body) {
  if (error) {
    console.error(error.stacktrace);
    process.exit(1);
  }

  if (response.statusCode !== 200) {
    console.error("Fetching failed with status code %s", response.statusCode);
    process.exit(2);
  }

  var lines = body.split("\n");
  dumpCerts(parseCertData(lines));
});
