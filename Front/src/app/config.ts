export interface ConfigAttributes {
    BaseApi: string;
    HubApi: string;
    HasDebug: boolean;
}

let config: ConfigAttributes = {
    BaseApi: process.env.BASE_API || 'http://localhost:5130/api/',
    HubApi: process.env.HUB_API || 'http://localhost:5130/gamehub',
    HasDebug: true,
};
if (process.env.NODE_ENV && process.env.NODE_ENV === 'production') {
    // production code
    config = {
        BaseApi: window.location.origin + '/api',
        HubApi: window.location.origin + '/gamehub',
        HasDebug: false,
    };
}

export default config;
