import React, { useState } from "react";
import AceEditor from "react-ace";
import { TextArea, Grid, Form, Header, Button, List } from "semantic-ui-react";
import aceModeList from "ace-builds/src-noconflict/ext-modelist"
import aceThemeList from "ace-builds/src-noconflict/ext-themelist"
import { JDoodleApiTestService } from "../swagger";
aceModeList.modes.forEach((x: any) => require(`ace-builds/src-noconflict/mode-${x.name}`));
aceThemeList.themes.forEach((x: any) => require(`ace-builds/src-noconflict/theme-${x.name}`));


type MyList = {
    key: number,
    value: string
}

export const ReactAceTest = () => {

    const [selectedLanguage, setSelectedLanguage] = useState<string>("java");
    const [selectedMode, setSelectedMode] = useState<string>("java");
    const [selectedTheme, setSelectedTheme] = useState<string>("twilight");
    const [selectedFontSize, setSelectedFontSize] = useState<number>(14);
    const [output, setOutput] = useState<string>("");
    const [input, setInput] = useState<Array<MyList>>([{ key: 1, value: "" }]);
    const [code, setCode] = useState<string>(`public class CompProgEdu {\n\tpublic static void main(String args[]) {\n\tint beep=1;\n\tint boop=2;\n\tint bop=3;\n\n\tSystem.out.println("Compiling.... beep... boop.. and bop. = " + beep+boop+bop);\n\t}\n}`);
    const [loading, setLoading] = useState(false);

    const selectLanguage = (e: any, data: any) => {
        const objectData = data.options.find((o: any) => o.value === data.value)
        setSelectedLanguage(objectData.key);
        setSelectedMode(objectData.value)
        setCode(objectData.defaultScript);
    }

    const selectTheme = (e: any, data: any) => {
        setSelectedTheme(data.value);
    }

    const selectFontSize = (e: any, data: any) => {
        setSelectedFontSize(data.value);
    }

    const staticLangOption = [//TODO MAKE THIS AN IMPORTABLE CONST FROM SOMEWHERE ELSE
        { key: "c", value: "c_cpp", text: "C", defaultScript: `#include<stdio.h>\n\nint main() {\n\tint beep=1;\n\tint boop=2;\n\tint bop=3;\n\n\tint total=beep+boop+bop;\n\n\tprintf("Compiling.... beep... boop.. and bop. = %i", total);\n}` },
        { key: "cpp", value: "cpp", text: "C++", defaultScript: `#include <iostream>\n\nusing namespace std;\n\nint main() {\n\tint beep=1;\n\tint boop=2;\n\tint bop=3;\n\ncout<<"Compiling.... beep... boop.. and bop. = " << beep+boop+bop;\n}` },
        { key: "csharp", value: "csharp", text: "C#", defaultScript: `using System;\n\nclass CompProgEdu\n{\n\tstatic void Main() {\n\tint beep = 1;\n\tint boop = 2;\n\tint bop = 3;\n\n\tConsole.Write("Compiling.... beep... boop.. and bop. = " + beep+boop+bop);\n}\n}\n` },
        { key: "java", value: "java", text: "Java", defaultScript: `public class CompProgEdu {\n\tpublic static void main(String args[]) {\n\tint beep=1;\n\tint boop=2;\n\tint bop=3;\n\n\tSystem.out.println("Compiling.... beep... boop.. and bop. = " + beep+boop+bop);\n\t}\n}` },
    ]

    const themeOptions = aceThemeList.themes.map((x: any) => ({ key: x.name, value: x.name, text: x.caption }))

    let fontOptions = [{}];
    const execute = async () => {
        var stdListString = input.map(x => x.value);
        setLoading(true);
        var result = await JDoodleApiTestService.createNew({ body: { script: code, language: selectedLanguage, versionIndex: "0", inputs: stdListString } })
        setLoading(false);
        if (result.result) {
            setOutput(result.result.output);
        }
        else {
            setOutput("Error");
        }

    }

    for (let i = 11; i < 33; i++) {
        fontOptions.push({ key: i, value: i, text: i.toString() + "pt" })
    }

    return (
        <div style={{ marginTop: "125px", marginLeft: "30px", marginRight: "30px" }}>
            <Grid >
                <Grid.Row columns={2} >
                    <Grid.Column width={13}>
                        <div style={{ textAlign: 'center' }}>
                            <Header style={{ color: "white" }} as='h1' content='CompProgEdu Sandbox Compiler IDE' />
                            <AceEditor
                                placeholder="Code here bro!"
                                mode={selectedMode}
                                theme={selectedTheme}
                                name="blah2"
                                fontSize={selectedFontSize}
                                showPrintMargin={true}
                                defaultValue={code}
                                value={code}
                                showGutter={true}
                                highlightActiveLine={true}
                                width={"1500px"}
                                minLines={25}
                                maxLines={45}
                                debounceChangePeriod={100}
                                onChange={(value) => { setCode(value); }}
                                setOptions={{
                                    enableBasicAutocompletion: true,
                                    enableLiveAutocompletion: true,
                                    enableSnippets: false,
                                    showLineNumbers: true,
                                    tabSize: 2,
                                }} />
                        </div>
                        <div style={{ marginTop: "40px", textAlign: 'center' }}>
                            <Header style={{ color: "white" }} as='h3' content='Output' />
                            <TextArea style={{ backgroundColor: "#141414", color: "white", width: "750px", height: "140px" }} value={output} />
                        </div>
                        <div style={{ textAlign: 'center' }}>
                            <Button loading={loading} disabled={loading} onClick={() => execute()}>Execute</Button>
                            <Button loading={loading} disabled={loading} onClick={() => setOutput("")}>Clear Console</Button>
                        </div>
                    </Grid.Column>
                    <Grid.Column width={3} >
                        <div style={{ marginTop: "40px" }}>
                            <div>
                                <label style={{ color: "white" }}>Select Language</label>
                                <Form.Dropdown defaultValue="java" selection onChange={selectLanguage} options={staticLangOption}></Form.Dropdown>
                            </div>
                            <div style={{ marginTop: "10px" }}>
                                <label style={{ color: "white" }}>Select Theme</label>
                                <Form.Dropdown selection defaultValue="twilight" onChange={selectTheme} options={themeOptions}></Form.Dropdown>
                            </div>
                            <div style={{ marginTop: "10px" }}>
                                <label style={{ color: "white" }}>Font Size</label>
                                <Form.Dropdown selection defaultValue={14} onChange={selectFontSize} options={fontOptions}></Form.Dropdown>
                            </div>
                            <List style={{ marginTop: "120px" }} ordered>
                                {input.sort((a, b) => (a.key > b.key) ? 1 : ((b.key > a.key) ? -1 : 0)).map(x =>
                                    <StdInput
                                        number={x.key}
                                        initialValue={x.value}
                                        disable={x.key === 1 ? true : false}
                                        subme={(value: MyList) => {

                                            var z = input.filter(x => x.key !== value.key)

                                            setInput(z)
                                        }}

                                        addme={(value: MyList) => {
                                            if (input.find(x => x.key === value.key + 1)) {
                                                var mapped = input.map(item => {
                                                    if (item.key > value.key) { return item = { key: item.key + 1, value: item.value } as MyList }
                                                    else { return item }
                                                })
                                                var newMap = { key: value.key + 1, value: "" }
                                                setInput([...mapped, newMap]);
                                            }
                                            else {
                                                var elseMap = { key: value.key + 1, value: "" }
                                                setInput([...input, elseMap]);
                                            }
                                        }}
                                        changeme={(value: MyList) => {
                                            var tes =
                                                input.map(q => {

                                                    if (q.key === value.key) { return q = { key: q.key, value: value.value } as MyList }
                                                    else { return q }
                                                })
                                            setInput(tes);
                                        }}
                                    />
                                )}
                            </List>
                        </div>
                    </Grid.Column>
                </Grid.Row>

            </Grid>
        </div >
    )
}


type InputMe = {
    number: number,
    initialValue: string,
    disable: boolean,
    addme: (value: MyList) => void,
    subme: (value: MyList) => void,
    changeme: (value: MyList) => void,
}
export const StdInput = (props: InputMe) => {
    return (
        <div>
            <label style={{ color: "white" }}>{`Std Input`}</label>
            <div style={{ display: "flex", flexDirection: "row" }}>
                {/* ADD DEBOUNCE TO THIS ONCHANGE LATER */}
                <Form.Input onChange={(e, d) => { props.changeme({ key: props.number, value: d.value }); }} value={props.initialValue} />
                <div style={{ marginLeft: "5px" }} >
                    <Button onClick={() => props.addme({ key: props.number, value: "" })} color='black' basic inverted >+</Button>
                    <Button onClick={() => props.subme({ key: props.number, value: "" })} color='black' basic disabled={props.disable} inverted >-</Button>
                </div>
            </div>
        </div>
    )
}