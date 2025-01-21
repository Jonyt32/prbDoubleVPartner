import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PersonasService } from 'src/app/services/personas.service';

@Component({
  selector: 'app-persona',
  templateUrl: './persona.component.html',
  styleUrls: ['./persona.component.scss']
})
export class PersonaComponent {
  personaForm: FormGroup;
  
  constructor(
    private fb: FormBuilder,
    private personasService: PersonasService
  ) {
    this.personaForm = this.fb.group({
      nombre: ['', Validators.required],
      apellido: ['', Validators.required],
      correo: ['', [Validators.required, Validators.email]],
      telefono: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.personaForm.valid) {
      this.personasService.addPersona(this.personaForm.value).subscribe(
        (response) => {
          console.log('Persona añadida', response);
        },
        (error) => {
          console.error('Error al agregar persona', error);
        }
      );
    }
  }
}
