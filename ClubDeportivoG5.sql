-- ============================================
-- BASE DE DATOS: CLUB DEPORTIVO
-- Grupo 5 DSOO Comisión C
-- ============================================

-- Eliminar base de datos si existe
DROP DATABASE IF EXISTS clubdeportivoG5;
CREATE DATABASE clubdeportivoG5;
USE clubdeportivoG5;

-- ============================================
-- VERIFICACIÓN
-- ============================================

SHOW TABLES;
SELECT * FROM persona;
SELECT * FROM usuario;
SELECT * FROM socio;
SELECT * FROM nosocio;
SELECT * FROM actividad;
SELECT * FROM cuota;
SELECT * FROM pago;
SELECT 'Tablas creadas exitosamente' AS Status;

TRUNCATE TABLE persona;
DROP TABLE persona;
TRUNCATE TABLE usuario;
DROP TABLE usuario;
TRUNCATE TABLE socio;
DROP TABLE socio;
TRUNCATE TABLE nosocio;
DROP TABLE nosocio;
TRUNCATE TABLE actividad;
DROP TABLE actividad;
TRUNCATE TABLE cuota;
DROP TABLE cuota;
TRUNCATE TABLE pago;
DROP TABLE pago;


-- ============================================
-- TABLA: Persona (Clase abstracta base)
-- ============================================
CREATE TABLE persona (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    dni VARCHAR(20) NOT NULL UNIQUE,
    telefono VARCHAR(20),
    email VARCHAR(100),
    tipo_persona ENUM('Socio', 'NoSocio') NOT NULL,
    fecha_registro DATETIME DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_dni (dni),
    INDEX idx_tipo (tipo_persona)
) ENGINE=InnoDB;

-- ============================================
-- TABLA: Usuario (para Login de TODOS)
-- MODIFICADA: Ahora soporta Socio, NoSocio, Administrador
-- ============================================
CREATE TABLE usuario (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    rol ENUM('Administrador', 'Empleado', 'Socio', 'NoSocio') DEFAULT 'Socio',
    activo BOOLEAN DEFAULT TRUE,
    persona_id INT NULL,
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (persona_id) REFERENCES persona(id) ON DELETE SET NULL,
    INDEX idx_username (username),
    INDEX idx_rol (rol)
) ENGINE=InnoDB;

-- ============================================
-- TABLA: Socio
-- ============================================
-- Eliminar tabla socio si existe
DROP TABLE IF EXISTS socio;

-- Crear tabla socio con todos los campos necesarios
CREATE TABLE socio (
    nroSocio INT AUTO_INCREMENT PRIMARY KEY,
    persona_id INT NOT NULL,
    fechaAlta DATE NOT NULL,
    habilitado BOOLEAN DEFAULT TRUE,
    aptofisico BOOLEAN DEFAULT FALSE,
    carnet VARCHAR(50),
    
    -- Campos para Stripe
    stripe_session_id VARCHAR(255),
    stripe_payment_intent_id VARCHAR(255),
    estado_pago ENUM('Pendiente', 'Completado', 'Fallido') DEFAULT 'Pendiente',
    fecha_pago DATETIME,
    monto_pago DOUBLE DEFAULT 100000.00,
    
    FOREIGN KEY (persona_id) REFERENCES persona(id) ON DELETE CASCADE,
    UNIQUE KEY uk_persona_socio (persona_id)
) ENGINE=InnoDB;

-- Verificar estructura
DESCRIBE socio;
SELECT * FROM socio;

-- ============================================
-- TABLA: NoSocio
-- ============================================
CREATE TABLE nosocio (
    id INT AUTO_INCREMENT PRIMARY KEY,
    persona_id INT NOT NULL,
    habilitado BOOLEAN DEFAULT TRUE,
    fechaVisita DATE,
    FOREIGN KEY (persona_id) REFERENCES persona(id) ON DELETE CASCADE,
    UNIQUE KEY uk_persona_nosocio (persona_id)
) ENGINE=InnoDB;

-- ============================================
-- TABLA: Actividad
-- ============================================
CREATE TABLE actividad (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion TEXT,
    horario VARCHAR(50),
    profesor VARCHAR(100),
    nivel VARCHAR(50),
    costo DOUBLE NOT NULL DEFAULT 0,
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- ============================================
-- TABLA: Cuota
-- ============================================
CREATE TABLE cuota (
    keySocio INT AUTO_INCREMENT PRIMARY KEY,
    socio_id INT NOT NULL,
    monto DOUBLE NOT NULL,
    fechaVencimiento DATE NOT NULL,
    estado VARCHAR(20) DEFAULT 'Pendiente',
    tipoDeCuota VARCHAR(50),
    metodoPago VARCHAR(50),
    FOREIGN KEY (socio_id) REFERENCES socio(nroSocio) ON DELETE CASCADE,
    INDEX idx_vencimiento (fechaVencimiento),
    INDEX idx_estado (estado)
) ENGINE=InnoDB;

-- ============================================
-- TABLA: Pago
-- ============================================
CREATE TABLE pago (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fechaPago DATE NOT NULL,
    monto DOUBLE NOT NULL,
    metodoPago VARCHAR(50),
    cuota_id INT,
    socio_id INT,
    FOREIGN KEY (cuota_id) REFERENCES cuota(keySocio) ON DELETE SET NULL,
    FOREIGN KEY (socio_id) REFERENCES socio(nroSocio) ON DELETE CASCADE
) ENGINE=InnoDB;


-- ============================================
-- DATOS DE PRUEBA
-- ============================================

-- Usuario Administrador por defecto
-- Password: admin123 (hasheada con BCrypt cuando se use desde la app)
INSERT INTO usuario (username, password, rol, persona_id) VALUES 
('admin', '$2a$12$QGicci8rd7LKz5.67MH0b.r2AEniq1gQJkRwr/Q2ojQ7DPesyksBm', 'Administrador', NULL);

-- Actividades de ejemplo
INSERT INTO actividad (nombre, descripcion, horario, profesor, nivel, costo) VALUES
('Fútbol', 'Clases de fútbol para todas las edades', 'Lun-Mié-Vie 18:00', 'Juan Pérez', 'Intermedio', 2000),
('Natación', 'Clases de natación', 'Mar-Jue 19:00', 'María González', 'Principiante', 2500),
('Gimnasio', 'Acceso a sala de musculación', 'Lun a Vie 8:00-22:00', 'Varios', 'Todos', 3000);