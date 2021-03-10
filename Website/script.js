let dhtdata = document.getElementById('dhtdata')

fetch("https://iot-fa.azurewebsites.net/api/GetDataFromCosmosDb")
.then(res => res.json())
.then(data => {
    for(let row of data) {
        dhtdata.innerHTML += `<td>${row.deviceid}</td>
        <td>${row.UtcTime}</td><td>${row.data.temp}</td><td>${row.data.hum}</td>`
    }
})

let shkdata = document.getElementById('shkdata')

fetch("https://iot-fa.azurewebsites.net/api/GetDataFromTable?code=xlLABHPIBJDR0hlTABOhKIaqHwMj6aD08CLrXwRWTYakdLN3kNkCKQ==&limit=10&orderby=desc")
.then(res => res.json())
.then(data => {
    for(let row of data) {
        shkdata.innerHTML += `<td>${row.deviceId}</td>
        <td>${row.utcTime}</td><td>${row.message}</td><td>${row.time}</td>`
    }
})

