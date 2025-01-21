// src/app/services/personas.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Persona } from '../models/persona.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PersonasService {
  private apiUrl = `${environment.apiUrl}/personas`;

  constructor(private http: HttpClient) { }

  // Obtener lista de personas
  getPersonas(): Observable<Persona[]> {
    return this.http.get<Persona[]>(this.apiUrl);
  }

  // Crear una nueva persona
  addPersona(persona: Persona): Observable<Persona> {
    return this.http.post<Persona>(this.apiUrl, persona);
  }
}
