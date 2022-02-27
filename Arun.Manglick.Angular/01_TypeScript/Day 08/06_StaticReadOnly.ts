class BankAccount{
    private static bankName = "BOA";
    private readonly _accNumber: number;

    constructor(){
        this._accNumber = 1;
    }

    get BankName(){
        return BankAccount.bankName;
    }

    get AccNumber(){
        return this._accNumber;
    }
}

var a1 = new BankAccount();
console.log(a1.BankName);
console.log(a1.AccNumber);

var a2 = new BankAccount();
console.log(a1.BankName);
