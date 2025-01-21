// src/app/services/login.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Usuario } from '../models/usuario.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LoginAppService {
  private apiUrl = `${environment.apiUrl}/login`;

  constructor(private http: HttpClient) { }

  // Método de login
  login(usuario: Usuario): Observable<any> {
    return this.http.post<any>(this.apiUrl, usuario);
  }
}
