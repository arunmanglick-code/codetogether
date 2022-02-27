// import { platformBrowserDynamic } from "@angular/platform-browser-dynamic";
// import { AppModule } from './app.module';

// // To avoid usage of augury chrome extenstion in chrome
// // import { enableProdMode } from "@angular/core";
// // enableProdMode();

// platformBrowserDynamic().bootstrapModule(AppModule);

// https://1drv.ms/f/s!Ao-ceLq5rZm3hTGAc4rNwePHJzLI

import { platformBrowser } from "@angular/platform-browser";
import { AppModuleNgFactory } from '../app/app.module.ngfactory';

platformBrowser().bootstrapModuleFactory(AppModuleNgFactory);