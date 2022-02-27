export interface IPoint1 {
    getDistance(): number;
}

export class Point1 implements IPoint1 {
    constructor(public x: number, public y: number) { }
    getDistance() {
        return Math.sqrt(this.x * this.x + this.y * this.y);
    }
}