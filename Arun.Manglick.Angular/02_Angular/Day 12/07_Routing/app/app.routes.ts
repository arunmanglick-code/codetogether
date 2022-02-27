import { Routes } from "@angular/router";

import { ListComponent } from "./ListComponent/app.listComponent";
import { QuoteComponent } from "./QuoteComponent/app.quoteComponent";

export const routes: Routes = [
    { path: 'list', component: ListComponent},
    { path: 'quote', component: QuoteComponent},
    { path: 'quote/:name', component: QuoteComponent}
];