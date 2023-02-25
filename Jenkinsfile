pipeline 
{
    environment 
    {
        // This registry is important for removing the image after the tests
        registry = "antonlunev/showapp"
    }
    agent any
    stages 
    {
        stage("Build image")
        {
            steps
            {
                script
                {
                    // Building the Docker image
                    dockerImage = docker.build(registry + ":$BUILD_NUMBER")
                }
            }
        }
        
        stage("Test") 
        {
            steps 
            {
                script 
                {
                    bool testPassed = true
                    try 
                    {
                        dockerImage.inside() 
                        {
                            // Extracting the SOURCEDIR environment variable from inside the container
                            def SOURCEDIR = sh(script: 'echo \$SOURCEDIR', returnStdout: true).trim()

                            // Running the tests inside the new directory
                            dir("$SOURCEDIR") 
                            {
                                sh "dotnet test ./showapp.tests/"
                            }
                        }
                    } 
                    catch (Exception ex)
                    {
                        testPassed = false
                    }

                    if (testPassed)
                    {
                        dockerImage.inside() 
                        {
                            // Extracting the APPDIR environment variable from inside the container
                            def APPDIR = sh(script: 'echo \$APPDIR', returnStdout: true).trim()
                            
                            dir("$SOURCEDIR") 
                            {
                                // Deploy app to /app
                                sh "dotnet publish ./showapp/ -c Release -r ubuntu.22.04-x64 --self-contained true -o $APPDIR"
                            }
                            
                            // Remove sourcecode
                            sh "rm -rf $SOURCEDIR"
                        }
                    }
                }
            }
        }
        stage("Upload image to repo")
        {
            steps
            {
                script
                {
                    if (testPassed)
                    {
                        // Push generated docker image to the registry
                        dockerImage.push()
                        // Also push it with a "latest" tag
                        dockerImage.push('latest')
                    }
                }
            }
        }
        stage("Cleanup")
        {
            steps
            {
                script
                {
                    // Removing the docker image
                    sh "docker rmi $registry:$BUILD_NUMBER"
                }
            }
        }
    }
}