/*var sayi=5;              değişken tanımlama

var name="John";            
const pi=3.14;          değiştirilemez 

var meyveler=["elma","armut","muz","çilek"];

function selamVer(isim){
    console.log("Merhaba"+isim+"!");
}

if(sayi > 3){
    console.log("Sayı 3'ten büyüktür.")
}
else{
    console.log("Girilen sayı 3'ten küçük veya eşittir.")
}

for(var i=0;i<meyveler.length;i++){
    console.log(meyveler[i]);
}

selamVer("Sena");           fonksiyon çağırımı*/


function Ekle(){
    var alisverisEkle=document.getElementById("alisverisEkle");
    var liste=document.getElementById("liste");
    var yeniOge=document.createElement("li");
    yeniOge.innerText=alisverisEkle.value;

//Ekleme
    if(alisverisEkle.value!==""){
        liste.appendChild(yeniOge);
        alisverisEkle.value ="";

       // Güncelleme işlemi
      yeniOge.onclick=function(){
        var yeniMetin=prompt("yeni değeri giriniz:");
        if(yeniMetin!==null & yeniMetin!==""){
            this.firstChild.nodeValue=yeniMetin;
        }
      }


//Tekil SİLME
        yeniOge.addEventListener("contextmenu",function(e) {   /* eklenilen öğeler üzerine gelinip sağ tıklandığında tekli silinmesi için*/
              e.preventDefault();
              this.parentNode.removeChild(yeniOge);
        } ); 
        
    }else{
           alert("Lütfen bir öğe giriniz!");
    }
}
//Tüm listeyi silme
 function listeTemizle(){
    var liste=document.getElementById("liste");
    liste.innerHTML="";                            /* butona basıldığında tüm öğeleri silme*/
 }




