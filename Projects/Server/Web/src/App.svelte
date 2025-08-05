<script>
import { SceneEnum, storeGlobalInfo } from "./store.js";	
import Login from "./Login.svelte";
import Home from "./Home.svelte";
</script>


{#if $storeGlobalInfo.Scene == SceneEnum.Login}
	<Login />
{:else if $storeGlobalInfo.Scene == SceneEnum.Menu}
	<Home />
{:else}
	<div>Error</div>
{/if}