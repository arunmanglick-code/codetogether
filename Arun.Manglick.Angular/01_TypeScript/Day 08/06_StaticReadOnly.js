var BankAccount = (function () {
    function BankAccount() {
        this._accNumber = 1;
    }
    Object.defineProperty(BankAccount.prototype, "BankName", {
        get: function () {
            return BankAccount.bankName;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankAccount.prototype, "AccNumber", {
        get: function () {
            return this._accNumber;
        },
        enumerable: true,
        configurable: true
    });
    BankAccount.bankName = "BOA";
    return BankAccount;
}());
var a1 = new BankAccount();
console.log(a1.BankName);
console.log(a1.AccNumber);
var a2 = new BankAccount();
console.log(a1.BankName);
