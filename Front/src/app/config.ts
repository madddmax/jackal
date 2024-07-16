

let config = {
    BaseApi: "http://localhost:5130/"
}
if (process.env.NODE_ENV && process.env.NODE_ENV === 'production') {
    // production code
    config = {
        BaseApi: "http://116.203.101.2/"
    }
}

export default config;
