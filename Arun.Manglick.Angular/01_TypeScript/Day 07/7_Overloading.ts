function myadd(x: number, y: number): number;
function myadd(x: number, y: number, z: number): number;
function myadd(x: number, y: number, z: number, a: number): number;

function myadd(): number {
    function M1(x: number, y: number): number {
        return x + y;
    }

    function M2(x: number, y: number, z: number): number {
        return x + y + z;
    }

    switch (arguments.length) {
        case 2:
            return M1(arguments[0], arguments[1]);
        case 3:
            return M2(arguments[0], arguments[1], arguments[2]);
        default:
            throw "No Method Implementation"
    }
}

console.log(myadd(2, 3));
console.log(myadd(2, 3, 4));

function check(x?: number, y?: number, z?: number): void{

}

check();
check(1);
check(1,2);
check(1,2,3);