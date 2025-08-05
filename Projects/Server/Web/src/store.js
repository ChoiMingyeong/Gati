import {readable, writable} from 'svelte/store';
import * as network from "../network.json";

export const SceneEnum = 
{
    Login: 0,
    Menu: 1,
}

export let storeGlobalInfo = writable({
    Scene: SceneEnum.Login,

    GetAdminServerUrl(apiUrl) 
    {
        return `http://${network['Host']}:${network['Port']}/InHouse/${apiUrl}`;
    },

    //https://svelte.dev/tutorial/kit/post-handlers

    // send api
    async InternalSendApiAsync(apiUrl, meth, request, cb)
    {
        var url = `http://${network['Host']}:${network['Port']}/InHouse/${apiUrl}`;
        console.log("url : ", url);
        const response = await fetch(url, {
            method: meth,
            body: request, //JSON.stringify({ request }),
            headers: {
                'Content-Type': 'application/json'
            }
        });
        cb(response.body);
    },

    async PostApiAsync(apiUrl, request, cb)
    {
        return await self.InternalSendApiAsync(apiUrl, 'post', request, cb);
    },

    async GetApiAsync(apiUrl, request, cb)
    {
        return await self.InternalSendApiAsync(apiUrl, 'get', request, cb);
    },
});