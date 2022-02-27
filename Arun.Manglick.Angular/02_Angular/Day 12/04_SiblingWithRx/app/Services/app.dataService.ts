import { Author } from "../Models/app.author";
import {Subject} from "rxjs/Subject";

export class DataService {
    private list: Array<Author>;
    private selectedAuthorS: Author;

    // Manish Approach
    // private authorSubject = new Subject();
    // authorSubjectChanged = this.authorSubject.asObservable();

    // Arun Approach
    public authorSubject = new Subject();

    constructor() {
        var fowler = {
            name: "Fowler",
            quote: "Any fool can write code that a computer can understand. Good programmers write code that humans can understand."
        },
            twain = {
                name: "Twain",
                quote: "Why, I have known clergymen, good men, kind-hearted, liberal, sincere, and all that, who did not know the meaning of a 'flush.' It is enough to make one ashamed of one's species."
            },
            poe = {
                name: "Poe",
                quote: "Deep into that darkness peering, long I stood there, wondering, fearing, doubting, dreaming dreams no mortal ever dared to dream before."
            },
            plato = {
                name: "Plato",
                quote: "All things will be produced in superior quantity and quality, and with greater ease, when each man works at a single occupation, in accordance with his natural gifts, and at the right moment, without meddling with anything else. "
            };

        this.list = [twain, fowler, poe, plato];
    }

    getAuthors() {
        return this.list;
    }

    setAuthor(a: Author) {
        this.selectedAuthorS = a;
        this.authorSubject.next();
    }

    getAuthor() {
        console.log(this.selectedAuthorS);
        return this.selectedAuthorS;
    }
}