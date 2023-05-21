// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let arrayCursos = [];
let inpInicio = document.getElementById("inpInicio");
let btnAlerta = document.getElementById("btnAlerta");
let frmCompra = document.getElementById("frmCompra");
let spanCant = document.getElementById("spanCant");
let res = false;
let totalCarrito = 0;
spanCant.innerHTML = 0;

var curso = new Object();

function agregar(precio, curso, id) {

    if (arrayCursos.length == 0) {

        curso = {
            name: curso,
            price: precio,
            id: id
        }
        arrayCursos.push(curso);
        spanCant.innerHTML = arrayCursos.length;


        dibujarTabla();

        localStorage.setItem('cursos', JSON.stringify(arrayCursos));

        document.getElementById("spnAlerta").innerHTML = "Curso agregado a tu carrito..";
        btnAlerta.click();
    }
    else {
        buscarId(id);
        if (res) {
            document.getElementById("spnAlerta").innerHTML = "Ya tienes agregado ese curso en tu carrito..";
            btnAlerta.click();
            res = false;
            return;
        }
        else {
            curso = {
                name: curso,
                price: precio,
                id: id
            }
            arrayCursos.push(curso);
            spanCant.innerHTML = arrayCursos.length;

            dibujarTabla();

            localStorage.setItem('cursos', JSON.stringify(arrayCursos));

            document.getElementById("spnAlerta").innerHTML = "Curso agregado a tu carrito..";
            btnAlerta.click();
        }
    }
    $("#btnIrAlCarrito").prop("disabled", false);
}

function buscarId(id) {
    arrayCursos = JSON.parse(localStorage.getItem("cursos"));
    arrayCursos.forEach(curso => {
        if (curso.id == id) {
            res = true;
        }
    });
}

function eliminar(pos) {

    arrayCursos.splice(pos, 1);
    spanCant.innerHTML = arrayCursos.length;

    localStorage.setItem('cursos', JSON.stringify(arrayCursos));


    if (arrayCursos.length == 0) {
        document.getElementsByClassName("modal-body")[0].innerHTML = "Tu carrito está vacío";
        localStorage.removeItem('cursos');
        loadCarrito(null);
        $("#btnIrAlCarrito").prop("disabled", true);
    }
    else {
        dibujarTabla();
        loadCarrito(arrayCursos);
    }
}

function dibujarTabla() {

    let modal = document.getElementById("modal-carrito");
    modal.innerHTML = "";
    let text = "";
    let suma = 0;

    text += "<table class='table table-striped'><thead><tr><th>Producto</th><th style='text-align: center;'>Precio</th><th style='text-align: center;'>Acciones</th></tr></thead><tbody>";
    let i;
    for (i = 0; i < arrayCursos.length; i++) {
        text += "<tr><td>" + arrayCursos[i].name + "</td><td style='text-align: center;'>" + arrayCursos[i].price + "</td><td style='text-align: center;'><button onclick='eliminar(" + i + ")' class='btn text-danger'><i class='bi bi-trash'></i></button></td></tr>";
        suma += arrayCursos[i].price;
    }

    text += "<tr><th>Total</th><th style='text-align: center;'>" + suma + "</th></tr></tbody></table>";

    modal.innerHTML = text;
}

function cargarCarrito() {

    if (localStorage.getItem('cursos') != null) {

        arrayCursos = JSON.parse(localStorage.getItem('cursos'));
        spanCant.innerHTML = arrayCursos.length;

        dibujarTabla();
    }
    else {
        let modal = document.getElementById("modal-carrito");
        modal.innerHTML = "Tu carrito está vacío..";
        $("#btnIrAlCarrito").prop("disabled", true);
    }
}

function comprar() {

    if (inpInicio.value == "0") {
        document.getElementById("spnCompra").innerHTML = "Para poder comprar tus cursos debes estar registrado e iniciar sesión..";
        btnCompra.click();
    }
    else {
        if (arrayId.length == 0) {
            document.getElementById("spnCompra").innerHTML = "Todavía no has agregado ningún curso a tu carrito..";
            btnCompra.click();
        }
        else {

            for (let i = 0; i < arrayId.length; i++) {
                let inpCursos = document.createElement("input");
                inpCursos.name = "inpCursos" + i;
                inpCursos.type = "hidden";
                inpCursos.value = arrayId[i];
                frmCompra.appendChild(inpCursos);
            }
            frmCompra.submit();

            vaciarCarrito();

        }
    }
}

function vaciarCarrito() {

    arrayCursos = [];
    spanCant.innerHTML = arrayCursos.length;

    document.getElementById("modal-carrito").innerHTML = "Tu carrito está vacío";
    localStorage.removeItem('cursos');
}

function loadCarrito(cursos) {
    if (cursos != null) {
        let text = "<table class='table table-striped'><thead><tr><th>Curso</th><th style='text-align: center;'>Cantidad</th><th style='text-align: center;'>Precio</th><th style='text-align: center;'>Acciones</th></tr></thead><tbody id='tbody'>";
        let suma = 0;

        cursos.forEach((curso, i) => {
            text += "<tr><td>" + curso.name + "</td><td style='text-align: center;'>1</td><td style='text-align: center;'>" + curso.price + "€</td><td style='text-align: center;'><button id='btnTrash' onclick='eliminar(" + i + ")' class='btn text-danger'><i class='bi bi-trash'></i></button></td></tr>";
            suma += curso.price;
        });
        text += "<tr><th colspan='3'>Total</th><th style='text-align: center;'>" + suma + "€</th></tr></tbody></table>";
        text += "<div class='d-flex justify-content-end'><button class='btn btn-success' id='btnContinuar'>Continuar</button></div>"
        $("#divCarrito").html(text);
    }
    else {
        $("#divCarrito").html("<h5>Tu carrito está vacío..</h5>")
    }

}

function loadCarritoPayment(cursos) {

    let text = "<table class='table table-striped'><thead><tr><th>Curso</th><th style='text-align: center;'>Cantidad</th><th style='text-align: center;'>Precio</th></tr></thead><tbody id='tbody'>";
    let suma = 0;

    cursos.forEach((curso, i) => {
        text += "<tr><td>" + curso.name + "</td><td style='text-align: center;'>1</td><td style='text-align: center;'>" + curso.price + "€</td></tr>";
        suma += curso.price;
    });
    text += "<tr><th colspan='2'>Total</th><th style='text-align: center;'>" + suma + "€</th></tr></tbody></table>";
    totalCarrito = suma;
    $("#divCarrito").html(text);

}

window.addEventListener("load", cargarCarrito());