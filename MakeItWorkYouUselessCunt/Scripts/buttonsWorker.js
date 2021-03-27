//Button
function isChecked() {
    var checkedFemale = document.getElementById('dot-2').checked;
    var checkedMale = document.getElementById('dot-1').checked;
    var error = document.getElementById('errorGender');

    if (checkedMale == false && checkedFemale == false) {
        error.innerText = 'You need to select a gender option!';
        return false;
    } else {
        return true;
    }
}

//Birth
function birthVal() {
    let now = new Date();
    let birth = document.getElementById('DateOfBirth');
    let birthObj = new Date(birth.value);
    if (((now.getFullYear() - birthObj.getFullYear()) < 18) || ((now.getFullYear() - birthObj.getFullYear()) > 100)) {
        var dateerror = "Eligible ages are between 18 and 100.";
        document.getElementById('dateError').innerText = dateerror;
        return false;
    } else {
        return true;
    }
}

//PDF
var err = document.getElementById('errorPDF');
var fil = document.getElementById('filePDF');

function validatePDF(objFileControl) {
    var file = objFileControl.value;
    var len = file.length;
    var ext = file.slice(len - 4, len);
    if (ext.toUpperCase() == ".PDF") {
        return true;
    }
    else {
        return false;
        err.innerHTML = "Only PDF files allowed.";
    }
}

function formOK() {
    if (validatePDF(fil) == false) {
        return false;
    } else {
        return true;
    }
}


//JPG
var err1 = document.getElementById('errorJPG');
var fil1 = document.getElementById('fileJPG');

function validateJPG(fil1) {
    var file = fil1.value;
    var len = file.length;
    var ext = file.slice(len - 4, len);
    if (ext.toUpperCase() == ".JPG") {
        return true;
    }
    else {
        return false;
        err1.innerHTML = "Only JPEG files allowed.";
    }
}

function formOK1() {
    if (validateJPG(fil1) == false) {
        return false;
    } else {
        return true;
    }
}

//Pic
document.getElementById("pic").style.display = "none";
function ChangePic(input) {
    var pic = document.getElementById("pic");
    var temp = pic.src;
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            pic.src = e.target.result;
        }
        reader.readAsDataURL(input.files[0]);
    } else {
        pic.src = temp;
    }
    pic.style.display = "block";
}