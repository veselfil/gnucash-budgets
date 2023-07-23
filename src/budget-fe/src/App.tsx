import './App.css';
import {OpenAPI} from "./gc-client";
import {GnuCashContextProvider} from "./context/GnuCashContext";
import 'bootstrap/dist/css/bootstrap.min.css';
import BudgetsSetupPage from "./pages/BudgetsSetupPage/BudgetsSetupPage";
import NavigationBar from "./components/NavigationBar/NavigationBar";
function App() {
  return (
      <GnuCashContextProvider>
          <NavigationBar />
          <BudgetsSetupPage />
      </GnuCashContextProvider>
  );
}

OpenAPI.BASE = "http://localhost:5192"

export default App;
