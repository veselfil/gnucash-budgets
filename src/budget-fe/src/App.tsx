import './App.css';
import {BudgetSetupLine} from "./components/BudgetSetupLine/BudgetSetupLine";
import {OpenAPI} from "./gc-client";
import {GnuCashContextProvider} from "./context/GnuCashContext";
import {AccountsList} from "./components/AccountsList/AccountsList";

function App() {
  return (
      <GnuCashContextProvider>
          <div className="App">
              <AccountsList />
          </div>      
      </GnuCashContextProvider>
  );
}

OpenAPI.BASE = "http://localhost:5192"

export default App;
