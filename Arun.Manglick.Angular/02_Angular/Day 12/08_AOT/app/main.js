"use strict";
// import { platformBrowserDynamic } from "@angular/platform-browser-dynamic";
// import { AppModule } from './app.module';
Object.defineProperty(exports, "__esModule", { value: true });
// // To avoid usage of augury chrome extenstion in chrome
// // import { enableProdMode } from "@angular/core";
// // enableProdMode();
// platformBrowserDynamic().bootstrapModule(AppModule);
// https://1drv.ms/f/s!Ao-ceLq5rZm3hTGAc4rNwePHJzLI
var platform_browser_1 = require("@angular/platform-browser");
var app_module_ngfactory_1 = require("../app/app.module.ngfactory");
platform_browser_1.platformBrowser().bootstrapModuleFactory(app_module_ngfactory_1.AppModuleNgFactory);
