module.export = class MyPerson{
    constructor(n){
        this.name = n;
    }

    setName(nm){
        this.name = nm;
    };

    getName(){
        return this.name;
    };
}

// export default MyPerson;

//Ref: https://stackoverflow.com/questions/39005332/include-es6-class-from-external-file-in-node-js
