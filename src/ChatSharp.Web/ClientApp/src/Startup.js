import { useEffect, useState } from "react";
import App from "./App";
import { get } from "./utils/HttpClient";
import Install from "./components/Install/Install";

const Startup = () => {
    let [databaseIsInstalled, setDatabaseIsInstalled] = useState(false);
    let [loading, setLoading] = useState(true);

    useEffect(() => {
        const PopulateComponent = async () => {
            const installResponse = await get(`Startup`);

            setLoading(false);
            if (installResponse.IsValid) {
                const startupData = installResponse.Data;

                setDatabaseIsInstalled(startupData.IsInstalled);
            }
        }

        PopulateComponent();
    }, []);

    return (
        <>
            {!loading && databaseIsInstalled &&
                <App />
            }

            {!loading && !databaseIsInstalled &&
                <Install />
            }
        </>
    )
}

export default Startup;