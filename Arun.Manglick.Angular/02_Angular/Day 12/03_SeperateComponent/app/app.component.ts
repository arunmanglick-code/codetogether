import { Component, OnInit, Inject } from '@angular/core';
import { Author } from './Models/app.author';
import { DataService } from './Services/app.dataService'

@Component({
  selector: 'my-app',
  template: ` <h2>Service Demo</h2>
                <hr/>
                <my-list></my-list>
                <my-quoteArea></my-quoteArea>
                `,
  styles: [require('./app.component.css').toString()],
  providers: [DataService]
})

export class RootComponent implements OnInit {

  ngOnInit() {
  }

}