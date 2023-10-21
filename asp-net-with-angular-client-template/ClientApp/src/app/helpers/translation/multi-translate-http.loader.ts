import { HttpClient } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { catchError, forkJoin, map, of } from 'rxjs';

export class MultiTranslateHttpLoader implements TranslateLoader {

    constructor(
      private http: HttpClient,
      public resources: Array<{ prefix: string, suffix?: string }> = [{
        prefix: '/assets/i18n/',
        suffix: '.json'
      }]) { }
  
  
    public getTranslation(lang: string): any {
  
      return forkJoin(this.resources.map(config => {
        return this.http.get(config.suffix ? `${config.prefix}${lang}${config.suffix}` : `${config.prefix}/${lang}`)
          .pipe(
            catchError((err) => of({}))
          );
      }))
        .pipe(
          map(response => {
            return response.reduce((a, b) => {
              return Object.assign(a, b);
            });
          }),
        );
    }
  }

  export function translateLoader(http: HttpClient): MultiTranslateHttpLoader {

    return new MultiTranslateHttpLoader(
      http,
      [
        { prefix: './assets/i18n/', suffix: '.json' }
      ]
    );
  }