<script>
    import { storeGlobalInfo } from "./store";

    let id="";
    let pw="";

    function Login()
    {
        var req = {
            Data : 0
        }
        $storeGlobalInfo.InternalSendApiAsync("GetData", "post", req, (res) => {
            alert("접속완료");
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