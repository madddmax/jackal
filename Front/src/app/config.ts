export interface ConfigAttributes {
    BaseApi: string;
    HasDebug: boolean;
}

let config: ConfigAttributes = {
    BaseApi: 'http://localhost:5130/api/',
    HasDebug: true,
};
if (process.env.NODE_ENV && process.env.NODE_ENV === 'production') {
    // production code
    config = {
        BaseApi: window.location.origin + '/api',
        HasDebug: false,
    };
}

export default config;
