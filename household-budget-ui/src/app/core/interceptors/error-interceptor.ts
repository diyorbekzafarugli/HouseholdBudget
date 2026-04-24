
import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const snackBar = inject(MatSnackBar);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let message = 'Произошла ошибка';

      if (error.error?.error) {
        message = error.error.error;
      } else {
        switch (error.status) {
          case 400: message = 'Неверные данные'; break;
          case 401: message = 'Необходима авторизация'; break;
          case 403: message = 'Доступ запрещён'; break;
          case 404: message = 'Не найдено'; break;
          case 409: message = 'Конфликт данных'; break;
          case 422: message = 'Ошибка бизнес-логики'; break;
          case 429: message = 'Слишком много запросов'; break;
          case 500: message = 'Ошибка сервера'; break;
        }
      }

      snackBar.open(message, 'Закрыть', {
        duration: 4000,
        panelClass: ['error-snackbar']
      });

      return throwError(() => error);
    })
  );
};