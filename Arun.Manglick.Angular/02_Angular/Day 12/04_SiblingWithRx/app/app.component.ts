import { Component } from '@angular/core';

import { DataService } from "./Services/app.dataService";

@Component({
  selector: 'my-app',
  template: ` <h2>Service Demo</h2>
              <hr/>
              <my-list></my-list>
              <quote></quote>
            `,
  providers: [DataService]
})
export class RootComponent { }