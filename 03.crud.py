#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Mon Oct 12 22:49:17 2020

@author: linuxmind
"""
import gi
import pyodbc
gi.require_version("Gtk", "3.0")
from gi.repository import Gtk

def Insertar(button):
    try:
        with conexion.cursor() as cursor:
            qry = "INSERT INTO pais(id, nombre, estatus) VALUES (?, ?, ?);"
            val = (txtId.get_text(), txtNombre.get_text(), "a")
            cursor.execute(qry, val)
            lblMensaje.set_text("!Registro Agregado Exitosamente!...")
            txtId.set_text("")
            txtNombre.set_text("")
    except:
        lblMensaje.set_text("!Error al insertar los datos!...")
        txtId.set_text("")
        txtNombre.set_text("")

def Modificar(button):
    try:
        with conexion.cursor() as cursor:

            qry = "UPDATE pais SET nombre = ? WHERE id = ?;"
            val = (txtNombre.get_text(), txtId.get_text())
            cursor.execute(qry, val)

        conexion.commit()
        lblMensaje.set_text("¡Registro Actualizado Exitosamente!...")
        txtId.set_text("")
        txtNombre.set_text("")
    except:
        lblMensaje.set_text("¡Fallo al Actualizar Registro!...")
        txtId.set_text("")
        txtNombre.set_text("")

def Borrar(button):
    try:
        with conexion.cursor() as cursor:

            qry = "DELETE FROM pais WHERE id = ?;"
            val = (txtId.get_text(),)
            cursor.execute(qry, val)

        conexion.commit()
        lblMensaje.set_text("¡Registro Eliminado Exitosamente!...")
        txtId.set_text("")
        txtNombre.set_text("")
    except:
        lblMensaje.set_text("¡Fallo al Eliminar Registro!...")
        txtId.set_text("")
        txtNombre.set_text("") 

def Buscar(button):
    try:
        with conexion.cursor() as cursor:
            qry = "SELECT id, nombre FROM pais WHERE id = ?;"
            val = (txtId.get_text(),)
            cursor.execute(qry, val)
            rdr = cursor.fetchall()

            if len(rdr) > 0:
                for valor in rdr:
                    txtNombre.set_text(valor[1])
                    lblMensaje.set_text("¡Busqueda Exitosa!")
            else:
                lblMensaje.set_text("¡Busqueda sin Registros!")
                txtId.set_text("")
                txtNombre.set_text("")                

    except:
        lblMensaje.set_text("¡Error Al Realizar la Busqueda!")
        txtId.set_text("")
        txtNombre.set_text("")

###################################################    
builder = Gtk.Builder()
builder.add_from_file("MainWindow.glade")
window = Gtk.Window()
window.connect("delete-event", Gtk.main_quit)

#Obtencion de los elementos del documento
lblId = builder.get_object("lblId")
lblNombre = builder.get_object("lblNombre")
lblconexion = builder.get_object("lblconexion")
lblMensaje = builder.get_object("lblMensaje")
txtId = builder.get_object("txtId")
txtNombre = builder.get_object("txtNombre")

#Botones
btnInsertar = builder.get_object("btnInsertar")
btnInsertar.connect("pressed", Insertar)

btnModificar = builder.get_object("btnModificar")
btnModificar.connect("pressed", Modificar)

btnBorrar = builder.get_object("btnBorrar")
btnBorrar.connect("pressed", Borrar)

btnBuscar = builder.get_object("btnBuscar")
btnBuscar.connect("pressed", Buscar)

##################################################
### Conexion con la base de datos      ###########
direccion_servidor = '127.0.0.1'
nombre_bd = 'test'
nombre_usuario = 'sa'
password = 'Canciondulce01!'
try:
    conexion = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};SERVER=' + direccion_servidor+';DATABASE='+nombre_bd+';UID='+nombre_usuario+';PWD=' + password)
    lblconexion.set_text("Conectado a la BD test...")
    # OK! conexión exitosa
except Exception as e:
    # Atrapar error
    lblconexion.set_text("Error al conectar con la BD...")
##################################################

window = builder.get_object("MainWindow")
window.show_all()

Gtk.main()