public class ObjectInfoHolder {
    private string key;
    private string value;

    public ObjectInfoHolder(string key, string value) {
        this.key = key;
        this.value = value;
    }

    public string GetInfoHolderKey() {
        return this.key;
    }

    public string GetInfoHolderValue() {
        return this.value;
    }
}
