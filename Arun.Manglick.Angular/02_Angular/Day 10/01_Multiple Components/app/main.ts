import { platformBrowserDynamic } from "@angular/platform-browser-dynamic";
import { AppModule } from './app.module';

// To avoid usage of augury chrome extenstion in chrome
// import { enableProdMode } from "@angular/core";
// enableProdMode();

platformBrowserDynamic().bootstrapModule(AppModule);