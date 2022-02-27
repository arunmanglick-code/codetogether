function myadd() {
    function M1(x, y) {
        return x + y;
    }
    function M2(x, y, z) {
        return x + y + z;
    }
    switch (arguments.length) {
        case 2:
            return M1(arguments[0], arguments[1]);
        case 3:
            return M2(arguments[0], arguments[1], arguments[2]);
        default:
            throw "No Method Implementation";
    }
}
console.log(myadd(2, 3));
console.log(myadd(2, 3, 4));
function check(x, y, z) {
}
check();
check(1);
check(1, 2);
check(1, 2, 3);
