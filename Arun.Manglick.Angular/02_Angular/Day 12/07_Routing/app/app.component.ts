import { Component } from '@angular/core';
import { DataService } from "./Services/app.dataService";

@Component({
  selector: 'my-app',
  template: ` <h2>Service Demo</h2>
              <hr/>
              <nav>
                <a routerLink="/list">List</a>
              </nav>
              <hr/>
              <router-outlet></router-outlet>
            `,
  providers: [DataService]
})
export class RootComponent { }