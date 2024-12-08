-- Inserciones para la tabla Sede
INSERT INTO Sede (id_Sede, nombre_sede, ciudad_sede, telefono_sede)
VALUES (1, 'FitCenter Norte', 'Bogot�', 5551111);

INSERT INTO Sede (id_Sede, nombre_sede, ciudad_sede, telefono_sede)
VALUES (2, 'FitCenter Sur', 'Medell�n', 5552222);

INSERT INTO Sede (id_Sede, nombre_sede, ciudad_sede, telefono_sede)
VALUES (3, 'FitCenter Este', 'Cali', 5553333);

-- Inserciones para la tabla CuentaSede
INSERT INTO CuentaSede (id_Sede, password_sede)
VALUES (1, 'password123');

INSERT INTO CuentaSede (id_Sede, password_sede)
VALUES (2, 'securepass456');

INSERT INTO CuentaSede (id_Sede, password_sede)
VALUES (3, 'fitpass789');

-- Inserciones para la tabla Persona
INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_sede)
VALUES (1, 'Juan P�rez', '3209021633', 'M',1);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_sede)
VALUES (2, 'Mar�a Garc�a', '3209021631', 'F',2);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_sede)
VALUES (3, 'Alex Rodr�guez', '3117428114', 'NE',3);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_sede)
VALUES (4, 'Paula Munoz', '3114578449', 'F',1);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_sede)
VALUES (5, 'Ana Argote', '3205462166', 'F',2);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_sede)
VALUES (6, 'Diana Delgado', '3265498566', 'F',3);

-- Inserciones para la tabla AreaEspecialidad
INSERT INTO AreaEspecialidad (id_AE, nombre_AE)
VALUES (1, 'Crossfit');

INSERT INTO AreaEspecialidad (id_AE, nombre_AE)
VALUES (2, 'Fuerza');

INSERT INTO AreaEspecialidad (id_AE, nombre_AE)
VALUES (3, 'Reduccion de peso');

-- Inserciones para la tabla Entrenador
INSERT INTO Entrenador (id_Entrenador, id_AE)
VALUES (1, 1);

INSERT INTO Entrenador (id_Entrenador, id_AE)
VALUES (2, 2);

INSERT INTO Entrenador (id_Entrenador, id_AE)
VALUES (3, 3);

-- Inserciones para la tabla CuentaEntrenador
INSERT INTO CuentaEntrenador (id_Entrenador, password_entrenador)
VALUES (1, 'entrenador123');

INSERT INTO CuentaEntrenador (id_Entrenador, password_entrenador)
VALUES (2, 'coach456');

INSERT INTO CuentaEntrenador (id_Entrenador, password_entrenador)
VALUES (3, 'trainer789');

-- Inserciones para la tabla Contrato
INSERT INTO Contrato (id_entrenador, salario, fecha_inicio_contrato, fecha_fin_contrato)
VALUES (1, 2500, TO_DATE('2023-01-01', 'YYYY-MM-DD'), TO_DATE('2024-12-31', 'YYYY-MM-DD'));

INSERT INTO Contrato (id_entrenador, salario, fecha_inicio_contrato, fecha_fin_contrato)
VALUES (2, 2800, TO_DATE('2023-02-01', 'YYYY-MM-DD'), TO_DATE('2025-01-31', 'YYYY-MM-DD'));

INSERT INTO Contrato (id_entrenador, salario, fecha_inicio_contrato, fecha_fin_contrato)
VALUES (3, 2600, TO_DATE('2023-03-01', 'YYYY-MM-DD'), NULL);

-- Inserciones para la tabla Cliente
INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (4, 1, TO_DATE('1990-05-15', 'YYYY-MM-DD'));

INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (5, 2, TO_DATE('1985-08-22', 'YYYY-MM-DD'));

INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (6, NULL, TO_DATE('1995-11-30', 'YYYY-MM-DD'));

-- Inserciones para la tabla Membresia
INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (4, TO_DATE('2023-01-15', 'YYYY-MM-DD'), 'Premium');

INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (5, TO_DATE('2023-02-20', 'YYYY-MM-DD'), 'Inactivo');

INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (6, TO_DATE('2023-03-25', 'YYYY-MM-DD'), 'General');

-- Inserciones para la tabla ProgresoFisico
INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (4, TO_DATE('2023-02-01', 'YYYY-MM-DD'), 75.5, 175, '120/80');

INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (5, TO_DATE('2023-03-01', 'YYYY-MM-DD'), 68.2, 165, '115/75');

-- Inserciones para la tabla PMF
INSERT INTO PMF (id_Cliente, fecha_valoracion, objetivo)
VALUES (4, TO_DATE('2023-02-01', 'YYYY-MM-DD'), 'Aumentar masa muscular');

INSERT INTO PMF (id_Cliente, fecha_valoracion, objetivo)
VALUES (5, TO_DATE('2023-03-01', 'YYYY-MM-DD'), 'Reducir grasa corporal');

-- Inserciones para la tabla NombreEjercicio
INSERT INTO NombreEjercicio (id_nombre_ejercicio, nombre_ejercicio)
VALUES (1, 'Squad');

INSERT INTO NombreEjercicio (id_nombre_ejercicio, nombre_ejercicio)
VALUES (2, 'Bench press');

INSERT INTO NombreEjercicio (id_nombre_ejercicio, nombre_ejercicio)
VALUES (3, 'Deadlifts');

-- Inserciones para la tabla Ejercicio
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (1, 4, TO_DATE('2023-02-01', 'YYYY-MM-DD'), 1, 12, 3, 'Lunes');

INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (2, 4, TO_DATE('2023-02-01', 'YYYY-MM-DD'), 2, 10, 4, 'Miercoles');

INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (3, 5, TO_DATE('2023-03-01', 'YYYY-MM-DD'), 3, 8, 5, 'Viernes');

INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (4, 5, TO_DATE('2023-03-01', 'YYYY-MM-DD'), 3, 8, 5, 'Sabado');