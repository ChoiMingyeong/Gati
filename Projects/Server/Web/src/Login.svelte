<script>
    import { SceneEnum, storeGlobalInfo } from "./store";

    let id="";
    let pw="";

    function Login()
    {
        var req = {
            ID : id,
            PW : pw
        }
        /*
        $storeGlobalInfo.InternalSendApiAsync("PostData2", "post", req, (res) => {
            alert("접속완료");
            //console.log("res : ", res);
            //console.log("res.result : ", res.result);
            //console.log("res.result.code : ", res.result.code);
            //console.log("res.result.message : ", res.result.message);
            //console.log("res.data : ", res.data);
        });
        */
        console.log("id : ", req.ID);
        console.log("pw : ", req.PW);
        alert("접속완료");
        $storeGlobalInfo.Scene = SceneEnum.Home;
    }
</script>
    <body>
        <div id="login">
            <div id="container">
                <div id="top">
                    <h2>  </h2>
                </div>
                <h2>Login</h2>
                <form method="post" id="login-form">
                    <div class="login_input">
                        <input bind:value={id} type="text" placeholder="ID" />
                        <input bind:value={pw} type="password" placeholder="PASSWORD" />
                    </div>
                    <div class="submit_button">
                        <input type="submit" on:click={Login} />
                    </div>
                </form>
            </div>
        </div>
    </body>
<style>

#login {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    min-width: 30%;
    min-height: 30%;
    background-color: rgb(236, 240, 245);
}

#container {
    width: 100%;
    max-width: 500px;
    height: 100%;
    align-items: center;
}

/* Styling the login form */
#login-form {
    display: inline-block;
    justify-content: center;
    text-align: center;
    align-items: center;
}

/* Styling the heading */
h2 {
    font-size: 35px;
    margin-bottom: 20px;
    margin-top: 10px;
}

/* Styling the input fields and submit button */
.login_input input {
    border: 2px solid #575757;
    border-radius: 40px;
    outline: none;
    width: 98%;
    height: 30px;
    text-indent: 7px;
    margin-bottom: 8px;
}

/* Styling the submit button */
.submit_button input[type="submit"] {
  width: 100%;
  padding: 10px;
  background-color: black;
  color: #fff;
  border: 1px solid #dbdbdb;
  border-radius: 40px;
  outline: none;
  font-size: 14px;
  box-sizing: border-box; /* 추가: 박스 모델에 패딩과 테두리 포함 */
}

/* Hover effect for the submit button */
.submit_button input[type="submit"]:hover {
  background-color: #575757;
}
</style>