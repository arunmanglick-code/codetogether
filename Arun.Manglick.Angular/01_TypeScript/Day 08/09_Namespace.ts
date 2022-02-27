namespace Synechron {
    export interface IPoint {
        getDistance(): number;
    }

    export class Point implements IPoint {
        constructor(public x: number, public y: number) { }
        getDistance() {
            return Math.sqrt(this.x * this.x + this.y * this.y);
        }
    }
}

namespace Usage {
    let point1: Synechron.IPoint = new Synechron.Point(2, 3);
    console.log(point1.getDistance());
}
