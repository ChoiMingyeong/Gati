<script>
    import { storeGlobalInfo } from "./store";

    let id="";
    let pw="";

    function Login()
    {
        var req = {
            Data : 0
        }
        $storeGlobalInfo.InternalSendApiAsync("PostData2", "post", req, (res) => {
            alert("접속완료");
            console.log(res.result.Code);
            console.log(res.data);
        });
    }
</script>

<body>
    <div id="login">
        <div id="container">
            <div id="top">
                <h2>TITLE</h2>
            </div>
            <div id="bottom">
                <div class="item">
                </div>
                <div class="item">
                    <input bind:value={id} type="email" placeholder="ID" />
                </div>
                <div class="item">
                    <input
                        bind:value={pw}
                        type="password"
                        placeholder="Password"
                    />
                </div>
                <div class="item"><button on:click={Login}>Login</button></div>
            </div>
        </div>
    </div>
</body>